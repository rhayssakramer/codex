using CodexAPI.Data;
using CodexAPI.Services;
using CodexAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adicionar services ao container
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Codex API",
        Version = "v1",
        Description = "API para gerenciar conteúdo educacional de programação",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Codex Team"
        }
    });

    // Adicionar suporte para JWT no Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configurar CORS
var corsOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(corsOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Configurar autenticação JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = builder.Configuration["Jwt:SecretKey"];

// Resolver variável de ambiente se é placeholder
if (!string.IsNullOrEmpty(secretKey) && secretKey.StartsWith("${"))
{
    var envVarName = secretKey.TrimStart('$', '{').TrimEnd('}');
    secretKey = Environment.GetEnvironmentVariable(envVarName);
}
if (string.IsNullOrEmpty(secretKey))
{
    secretKey = Environment.GetEnvironmentVariable("Jwt__SecretKey");
}

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JWT SecretKey não configurada. Defina Jwt__SecretKey como variável de ambiente.");
}

var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Append("X-Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

// Configurar banco de dados
var environment = builder.Environment.EnvironmentName;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Resolver variáveis de ambiente se o valor contém placeholder ${...}
if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("${"))
{
    var envVarName = connectionString.TrimStart('$', '{').TrimEnd('}');
    connectionString = Environment.GetEnvironmentVariable(envVarName);
}

// Fallback: tentar ler diretamente da variável de ambiente NEON_DATABASE_URL
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("NEON_DATABASE_URL")
        ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string não configurada. Defina NEON_DATABASE_URL ou ConnectionStrings__DefaultConnection.");
}

// Converter URI do PostgreSQL (postgresql://user:pass@host/db) para formato Npgsql
if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    var host = uri.Host;
    var port = uri.Port > 0 ? uri.Port : 5432;
    var database = uri.AbsolutePath.TrimStart('/');
    var username = userInfo[0];
    var password = userInfo.Length > 1 ? userInfo[1] : "";
    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}

if (environment == "Development")
{
    builder.Services.AddDbContext<CodexDbContext>(options =>
        options.UseSqlite(connectionString)
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));
}
else if (environment == "Homolog" || environment == "Production")
{
    // Usar PostgreSQL (Neon)
    builder.Services.AddDbContext<CodexDbContext>(options =>
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromMilliseconds(1000),
                errorCodesToAdd: null);
        })
            .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));
}

// Registrar repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
builder.Services.AddScoped<ITopicoRepository, TopicoRepository>();
builder.Services.AddScoped<IProgressoTopicoRepository, ProgressoTopicoRepository>();

// Registrar services
builder.Services.AddScoped<IAuthService, AuthService>();

// Health Checks
builder.Services.AddHealthChecks();

// Logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    if (environment == "Development")
    {
        config.AddDebug();
    }
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Codex API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Servir arquivos estáticos (imagens, etc)
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Adicionar headers CORS para arquivos estáticos
        ctx.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        ctx.Context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
        ctx.Context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
    }
});

// Usar CORS
app.UseCors("AllowSpecificOrigins");

// Middleware de segurança
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapHealthChecks("/health");

// Middleware para tratamento de exceções globais
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exceptionHandlerFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (exceptionHandlerFeature?.Error != null)
        {
            logger.LogError(exceptionHandlerFeature.Error, "Unhandled exception occurred");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            sucesso = false,
            mensagem = "Erro interno do servidor",
            erros = app.Environment.IsDevelopment() ? new[] { exceptionHandlerFeature?.Error?.Message } : null
        });
    });
});

// Inicializar banco de dados
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CodexDbContext>();
        
        // Aplicar migrations
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"Ambiente: {environment}");
        logger.LogInformation("Aplicando migrations...");

        await context.Database.MigrateAsync();

        logger.LogInformation("Migrations aplicadas com sucesso");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrations. O app continuará sem migrations.");
        // Não faz throw para não crashar o container - permite health check diagnosticar
    }
}

app.Run();

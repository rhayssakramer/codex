<div align="center">

# 🔧 CODEX Backend

**API REST Robusta para a Biblioteca de Conteúdo de Programação**

Backend em ASP.NET Core 9.0 com arquitetura limpa, segurança em primeiro lugar e suporte a múltiplos ambientes. Fornece toda a infraestrutura necessária para gerenciar usuários, conteúdos, tópicos e uploads de mídia.

[![Framework](https://img.shields.io/badge/Framework-ASP.NET%20Core%209.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com)
[![Language](https://img.shields.io/badge/Language-C%23%2013-239120?style=for-the-badge&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp)
[![Database](https://img.shields.io/badge/Database-PostgreSQL%2015+-336791?style=for-the-badge&logo=postgresql)](https://www.postgresql.org)
[![Deploy](https://img.shields.io/badge/Deploy-Render-46E3B7?style=for-the-badge&logo=render)](https://render.com)

</div>

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Instalação e Configuração](#-instalação-e-configuração)
- [Executando o Projeto](#-executando-o-projeto)
- [API Endpoints](#-api-endpoints)
- [Controllers](#-controllers)
- [Serviços](#-serviços)
- [Autenticação e Segurança](#-autenticação-e-segurança)
- [Banco de Dados](#-banco-de-dados)
- [Entity Framework Core](#-entity-framework-core)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Build e Deploy](#-build-e-deploy)
- [Monitoramento](#-monitoramento)
- [Troubleshooting](#-troubleshooting)
- [Padrões e Convenções](#-padrões-e-convenções)
- [Créditos](#-créditos)

---

## 🌟 Sobre o Projeto

O **CODEX Backend** é uma API REST completa que fornece toda a infraestrutura necessária para a plataforma CODEX. Desenvolvida em ASP.NET Core 9.0, oferece:

- ✅ **Autenticação JWT Stateless** - Tokens seguros com validade configurável
- ✅ **Gerenciamento de Usuários** - CRUD com permissões e roles
- ✅ **Gestão de Conteúdo** - CRUD para tópicos, disciplinas e conteúdos
- ✅ **Upload de Mídias** - Integração com Cloudinary para armazenamento seguro
- ✅ **Busca Avançada** - Filtros e busca full-text em conteúdos
- ✅ **Sistema de Permissões** - Controle granular de acesso
- ✅ **Logging Estruturado** - Rastreamento de eventos e erros
- ✅ **Health Checks** - Monitoramento de saúde da aplicação
- ✅ **API Documentation** - Swagger/OpenAPI automático
- ✅ **Múltiplos Ambientes** - SQLite (dev), PostgreSQL (homolog/prod)

---

## 🏛️ Arquitetura

O backend segue uma arquitetura **Clean Architecture** com separação clara de responsabilidades:

```
Presentation Layer (Controllers)
        ↓
Business Logic Layer (Services)
        ↓
Data Access Layer (Repositories)
        ↓
Database Layer (Entity Framework Core)
```

### Fluxo de Requisição

```
HTTP Request
    ↓
Controller (Validação de entrada)
    ↓
Service (Lógica de negócio)
    ↓
Repository (Acesso aos dados)
    ↓
DbContext (Entity Framework Core)
    ↓
Database (SQLite/PostgreSQL)
    ↓
Response (DTO serializado em JSON)
```

### Camadas da Aplicação

```
CodexAPI/
├── Controllers          # Camada de Apresentação
│   └── Recebem requisições HTTP e delegam para serviços
│
├── Services            # Camada de Lógica de Negócio
│   └── Contêm regras de negócio e orquestração
│
├── Repositories        # Camada de Acesso a Dados
│   └── Abstração sobre o DbContext
│
├── Models              # Entidades de Domínio
│   └── Representam objetos do banco de dados
│
├── DTOs                # Data Transfer Objects
│   └── Objetos de transferência de dados
│
├── Middleware          # Middleware Customizado
│   └── Processamento global de requisições
│
├── Data                # Configuração do EF Core
│   └── DbContext e Migrações
│
└── Program.cs          # Ponto de entrada e DI
```

---

## 💻 Tecnologias

### Core Framework

| Tecnologia | Versão | Descrição |
|-----------|--------|-----------|
| **ASP.NET Core** | 9.0 | Framework web moderno da Microsoft |
| **C#** | 13 | Linguagem de programação |
| **.NET SDK** | 9.0.x | Runtime e ferramentas de desenvolvimento |

### ORM e Banco de Dados

| Tecnologia | Versão | Descrição |
|-----------|--------|-----------|
| **Entity Framework Core** | 9.0 | ORM para acesso a dados |
| **PostgreSQL** | 15+ | Banco relacional (homolog/prod) |
| **SQLite** | — | Banco local para desenvolvimento |
| **Npgsql** | 8.0.0 | Driver PostgreSQL para .NET |

### Autenticação e Segurança

| Tecnologia | Versão | Descrição |
|-----------|--------|-----------|
| **JWT Bearer** | 7.0.0 | Autenticação com JWT tokens |
| **BCrypt.Net-Next** | 4.0.3 | Hashing seguro de senhas |
| **System.IdentityModel.Tokens.Jwt** | 7.0.0 | Processamento de JWT |

### Utilitários e Integração

| Tecnologia | Versão | Descrição |
|-----------|--------|-----------|
| **AutoMapper** | 12.0.1 | Mapeamento entre objetos |
| **FluentValidation** | 11.9.2 | Validação de dados |
| **Serilog** | 8.0.1 | Logging estruturado |
| **Cloudinary SDK** | 1.28.0 | Armazenamento de mídias |
| **MailKit** | 4.9.0 | Envio de e-mails |
| **Swashbuckle** | 6.5.0 | Documentação Swagger/OpenAPI |

### Desenvolvimento

| Ferramenta | Versão | Descrição |
|-----------|--------|-----------|
| **Visual Studio 2022** | 17.0+ | IDE (opcional) |
| **VS Code** | latest | Editor (opcional) |
| **Docker** | 24.0+ | Containerização |
| **Git** | 2.40+ | Controle de versão |

---

## 📁 Estrutura do Projeto

```
backend/
├── CodexAPI.csproj                    # Arquivo de projeto .NET
├── Program.cs                         # Ponto de entrada e configuração DI
├── Dockerfile                         # Imagem Docker
├── docker-compose.yml                 # Orquestração local (PostgreSQL)
├── global.json                        # Versão do SDK .NET obrigatória
├── nuget.config                       # Configuração de feeds NuGet
├── build.sh                           # Script de build para Linux
│
├── appsettings.json                   # Configurações gerais (dev)
├── appsettings.Development.json       # Configurações de desenvolvimento
├── appsettings.Homolog.json           # Configurações de homologação
├── appsettings.Production.json        # Configurações de produção
│
├── Controllers/                       # Camada de Apresentação
│   ├── AuthController.cs              # Autenticação (login, registro)
│   ├── ContentController.cs           # Gerenciamento de conteúdos
│   ├── SearchController.cs            # Busca e filtros
│   ├── TopicosController.cs           # Gerenciamento de tópicos
│   └── UploadsController.cs           # Upload de mídias
│
├── Services/                          # Camada de Lógica de Negócio
│   ├── AuthService.cs                 # Serviço de autenticação
│   ├── IAuthService.cs                # Interface do serviço de auth
│   └── (outros serviços conforme necessário)
│
├── Repositories/                      # Camada de Acesso a Dados
│   ├── GenericRepositories.cs         # Repository genérico
│   └── (repositórios específicos)
│
├── Models/                            # Entidades de Domínio
│   ├── DomainModels.cs                # Modelos do banco de dados
│   └── (entidades específicas)
│
├── DTOs/                              # Data Transfer Objects
│   ├── DtoModels.cs                   # Objetos de transferência
│   └── (DTOs específicas)
│
├── Data/                              # Configuração Entity Framework
│   ├── CodexDbContext.cs              # DbContext principal
│   └── Migrations/                    # Migrações do banco
│       ├── 20260513153937_InitialCreate.cs
│       ├── 20260513162257_AddAdminUser.cs
│       └── CodexDbContextModelSnapshot.cs
│
├── Middleware/                        # Middleware Customizado
│   └── SecurityHeadersMiddleware.cs    # Headers de segurança
│
├── Properties/                        # Metadados do projeto
│   └── launchSettings.json            # Configurações de launch profiles
│
├── scripts/                           # Scripts úteis
│   ├── setup.sh                       # Setup em Linux/macOS
│   └── setup.bat                      # Setup em Windows
│
├── wwwroot/                           # Arquivos estáticos
│   └── uploads/                       # Diretório de uploads (local)
│
├── Documentation/                     # Documentação interna
│   ├── ARCHITECTURE.md                # Detalhes de arquitetura
│   ├── DATABASE.md                    # Documentação do banco
│   ├── API.md                         # Guia de API
│   └── ...
│
├── bin/                               # Binários compilados
└── obj/                               # Objetos compilados

```

---

## 📌 Pré-requisitos

### Obrigatório

- [**.NET 9.0 SDK**](https://dotnet.microsoft.com/download/dotnet/9.0) - Runtime e ferramentas
- [**Git**](https://git-scm.com/) - Controle de versão

### Para Desenvolvimento

- **Visual Studio 2022** ou **VS Code** com C# Dev Kit (recomendado)
- **Postman** ou **Thunder Client** para testar endpoints

### Para Banco de Dados (Opcional em desenvolvimento)

- **PostgreSQL 15+** - Para ambiente local sem Docker
- **Docker Desktop** - Para executar PostgreSQL em container

### Conhecimentos Recomendados

- ASP.NET Core
- C# 13
- Entity Framework Core
- REST APIs
- JWT Authentication
- SQL/PostgreSQL

---

## 🔧 Instalação e Configuração

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/codex.git
cd codex/backend
```

### 2. Verificar Versão do .NET

```bash
dotnet --version
# Deve ser 9.0.x ou superior
```

Se não tiver .NET 9.0, o arquivo `global.json` forçará a instalação:

```bash
# O .NET instalará automaticamente via global.json
# Ou instale manualmente
dotnet new globaljson --sdk-version 9.0.100 --roll-forward latestMinor
```

### 3. Restaurar Dependências

```bash
dotnet restore
```

Isso irá:
- Baixar todos os pacotes NuGet
- Restaurar dependências do projeto
- Preparar o ambiente para compilação

### 4. Configuração do Banco de Dados

#### Desenvolvimento (SQLite - Recomendado)

Nenhuma configuração extra necessária! O SQLite será criado automaticamente.

```bash
dotnet ef database update
```

Isso criará o arquivo `codex.db` no diretório raiz.

#### Homolog/Produção (PostgreSQL)

Configure a string de conexão em `appsettings.Homolog.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=seu-host;Port=5432;Database=codex_homolog;Username=usuario;Password=senha;SslMode=Require"
  }
}
```

Ou via variável de ambiente:

```bash
export DATABASE_URL="postgresql://usuario:senha@host:5432/codex_homolog?sslmode=require"
```

### 5. Configurar Segurança

Defina a chave JWT:

```bash
# Linux/macOS
export JWT_SECRET="sua_chave_secreta_minimo_32_caracteres"

# Windows (PowerShell)
$env:JWT_SECRET="sua_chave_secreta_minimo_32_caracteres"

# Windows (CMD)
set JWT_SECRET=sua_chave_secreta_minimo_32_caracteres
```

Ou configure no `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "sua_chave_secreta_minimo_32_caracteres",
    "ExpirationMinutes": 1440
  }
}
```

### 6. Verificar Instalação

```bash
dotnet build
# Deve compilar sem erros
```

---

## 🚀 Executando o Projeto

### Desenvolvimento (SQLite com Hot Reload)

```bash
dotnet watch run
```

O servidor iniciará em:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:7000`
- **Swagger:** `http://localhost:5000/swagger`

### Desenvolvimento (Sem Hot Reload)

```bash
dotnet run
```

### Homolog (PostgreSQL)

```bash
dotnet run --launch-profile Homolog
```

Ou via variável de ambiente:

```bash
ASPNETCORE_ENVIRONMENT=Homolog dotnet run
```

### Produção (PostgreSQL)

```bash
ASPNETCORE_ENVIRONMENT=Production dotnet run
```

### Com Docker Compose (Backend + PostgreSQL)

```bash
docker-compose up --build
```

Será criado um container com:
- Backend em `http://localhost:5000`
- PostgreSQL em `localhost:5432`

Para parar:

```bash
docker-compose down
```

### Compilar para Publicação

```bash
# Debug
dotnet build

# Release
dotnet build --configuration Release

# Publicar
dotnet publish --configuration Release --output ./publish
```

---

## 📡 API Endpoints

A documentação interativa completa está disponível em **`http://localhost:5000/swagger`** quando a aplicação está em execução.

### Autenticação — `/auth`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/auth/registrar` | Registrar novo usuário | ❌ |
| POST | `/auth/login` | Login e obtenção JWT | ❌ |
| POST | `/auth/solicitar-redefinicao-senha` | Solicitar reset de senha | ❌ |
| POST | `/auth/redefinir-senha` | Redefinir senha com token | ❌ |
| GET | `/auth/me` | Dados do usuário autenticado | ✅ |
| GET | `/auth/verify` | Verificar validade do token | ✅ |

**Exemplo - Login:**
```bash
curl -X POST http://localhost:5000/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "usuario@exemplo.com",
    "senha": "senha123"
  }'
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tipo": "Bearer",
  "usuario": {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@exemplo.com",
    "tipo": "Admin"
  }
}
```

### Conteúdo — `/api/content`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/content` | Criar conteúdo | ✅ |
| GET | `/api/content` | Listar conteúdos | ❌ |
| GET | `/api/content/{id}` | Buscar conteúdo por ID | ❌ |
| PUT | `/api/content/{id}` | Atualizar conteúdo | ✅ |
| DELETE | `/api/content/{id}` | Deletar conteúdo | ✅ |
| GET | `/api/content/user/{userId}` | Conteúdos de um usuário | ❌ |

### Tópicos — `/api/topicos`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/topicos` | Criar tópico | ✅ |
| GET | `/api/topicos` | Listar tópicos | ❌ |
| GET | `/api/topicos/{id}` | Buscar tópico | ❌ |
| PUT | `/api/topicos/{id}` | Atualizar tópico | ✅ |
| DELETE | `/api/topicos/{id}` | Deletar tópico | ✅ |

### Busca — `/api/search`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/search?q={query}` | Buscar por palavra-chave | ❌ |
| GET | `/api/search/advanced` | Busca avançada com filtros | ❌ |
| GET | `/api/search/tags/{tag}` | Buscar por tag | ❌ |

### Uploads — `/api/uploads`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| POST | `/api/uploads` | Upload de arquivo | ✅ |
| GET | `/api/uploads` | Listar meus uploads | ✅ |
| DELETE | `/api/uploads/{id}` | Deletar arquivo | ✅ |

### Usuários — `/api/usuarios`

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/usuarios` | Listar todos | Admin |
| GET | `/api/usuarios/me` | Dados do autenticado | ✅ |
| GET | `/api/usuarios/{id}` | Buscar usuário | ✅ |
| PUT | `/api/usuarios/{id}` | Atualizar usuário | ✅ |
| DELETE | `/api/usuarios/{id}` | Deletar usuário | Admin |

Endpoints marcados com ✅ requerem o header:
```
Authorization: Bearer <seu_jwt_token>
```

---

## 🎮 Controllers

### AuthController

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Senha);
        return Ok(result);
    }
    
    [HttpPost("registrar")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _authService.GetUserAsync(userId);
        return Ok(user);
    }
}
```

### ContentController

```csharp
[ApiController]
[Route("api/[controller]")]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;
    
    [HttpGet]
    public async Task<ActionResult<List<ContentDto>>> GetAll()
    {
        var contents = await _contentService.GetAllAsync();
        return Ok(contents);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ContentDto>> GetById(int id)
    {
        var content = await _contentService.GetByIdAsync(id);
        return Ok(content);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ContentDto>> Create(CreateContentRequest request)
    {
        var content = await _contentService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = content.Id }, content);
    }
}
```

---

## 🔧 Serviços

### AuthService

```csharp
public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(string email, string password);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<UserDto> GetUserAsync(string userId);
    Task LogoutAsync(string userId);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtService;
    private readonly IPasswordHashService _passwordService;
    
    public async Task<AuthResponseDto> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null || !_passwordService.VerifyPassword(password, user.Senha))
            throw new UnauthorizedException("Email ou senha inválidos");
        
        var token = _jwtService.GenerateToken(user);
        
        return new AuthResponseDto
        {
            Token = token,
            Tipo = "Bearer",
            Usuario = new UserDto { /* ... */ }
        };
    }
}
```

### ContentService

```csharp
public interface IContentService
{
    Task<List<ContentDto>> GetAllAsync();
    Task<ContentDto> GetByIdAsync(int id);
    Task<ContentDto> CreateAsync(CreateContentRequest request);
    Task<ContentDto> UpdateAsync(int id, UpdateContentRequest request);
    Task DeleteAsync(int id);
}

public class ContentService : IContentService
{
    private readonly IContentRepository _contentRepository;
    private readonly IMapper _mapper;
    
    public async Task<ContentDto> CreateAsync(CreateContentRequest request)
    {
        var content = _mapper.Map<Content>(request);
        await _contentRepository.AddAsync(content);
        await _contentRepository.SaveChangesAsync();
        
        return _mapper.Map<ContentDto>(content);
    }
}
```

---

## 🔐 Autenticação e Segurança

### JWT Authentication

O backend usa **JWT (JSON Web Tokens)** para autenticação stateless:

```csharp
// Configuração em Program.cs
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
```

### Hashing de Senhas

Utiliza **BCrypt** para hash seguro:

```csharp
// Registrar senha
var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

// Verificar senha
bool isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
```

### Authorization Policies

```csharp
// Em Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
    
    options.AddPolicy("ContentCreator", policy =>
        policy.RequireRole("Admin", "ContentCreator"));
});

// Em Controller
[Authorize(Policy = "AdminOnly")]
public ActionResult DeleteUser(int id) { /* ... */ }
```

### Security Headers Middleware

```csharp
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        
        await _next(context);
    }
}

// Em Program.cs
app.UseMiddleware<SecurityHeadersMiddleware>();
```

---

## 🗄️ Banco de Dados

### Ambientes Suportados

| Ambiente | Banco | Conexão | Uso |
|----------|------|---------|-----|
| **Development** | SQLite | Local (arquivo) | Desenvolvimento local |
| **Homolog** | PostgreSQL (Neon) | Cloud | Testes em produção |
| **Production** | PostgreSQL (Neon) | Cloud | Aplicação em produção |

### SQLite (Desenvolvimento)

**Arquivo:** `codex.db` (criado automaticamente)

```bash
# Listar tabelas
sqlite3 codex.db ".tables"

# Consultar dados
sqlite3 codex.db "SELECT * FROM usuarios;"
```

### PostgreSQL (Homolog/Produção)

Hospedado em **[Neon](https://neon.tech)** (PostgreSQL serverless).

**Connection String Format:**
```
postgresql://usuario:senha@host:5432/database?sslmode=require
```

**Variáveis de Ambiente:**
```env
DATABASE_URL_HOMOLOG=postgresql://...
DATABASE_URL_PRODUCTION=postgresql://...
```

### String de Conexão em appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=codex_dev;User Id=postgres;Password=postgres;"
  }
}
```

---

## 🏗️ Entity Framework Core

### DbContext

```csharp
public class CodexDbContext : DbContext
{
    public CodexDbContext(DbContextOptions<CodexDbContext> options)
        : base(options) { }
    
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Conteudo> Conteudos { get; set; }
    public DbSet<Topico> Topicos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurações de modelo
        modelBuilder.Entity<Usuario>()
            .HasKey(u => u.Id);
        
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}
```

### Migrações

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Remover última migration
dotnet ef migrations remove

# Ver histórico de migrations
dotnet ef migrations list

# Revert para uma migration específica
dotnet ef database update Nome_da_Migration_Anterior

# Drop banco (cuidado!)
dotnet ef database drop
```

### Exemplo de Migration

```csharp
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Usuarios",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", 
                        NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Nome = table.Column<string>(type: "text", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                Senha = table.Column<string>(type: "text", nullable: false),
                Tipo = table.Column<int>(type: "integer", nullable: false),
                CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Usuarios", x => x.Id);
            });
    }
}
```

---

## 📦 Variáveis de Ambiente

### Autenticação

| Variável | Descrição | Exemplo | Obrigatório |
|----------|-----------|---------|-----------|
| `JWT_SECRET` | Chave para assinatura JWT | `sua_chave_super_secreta_minimo_32_chars` | ✅ |
| `JWT_EXPIRATION_MINUTES` | Minutos até expiração | `1440` (24h) | ❌ |

### Banco de Dados

| Variável | Descrição | Exemplo | Obrigatório |
|----------|-----------|---------|-----------|
| `DATABASE_URL_DEVELOPMENT` | ConnectionString SQLite | `Data Source=codex.db` | ❌ |
| `DATABASE_URL_HOMOLOG` | ConnectionString Homolog | `postgresql://user:pass@host/db` | ❌ |
| `DATABASE_URL_PRODUCTION` | ConnectionString Produção | `postgresql://user:pass@host/db` | ❌ |

### Cloudinary (Upload de Mídias)

| Variável | Descrição | Exemplo | Obrigatório |
|----------|-----------|---------|-----------|
| `CLOUDINARY_CLOUD_NAME` | Nome da cloud | `seu_cloud_name` | ✅ |
| `CLOUDINARY_API_KEY` | API Key | `1234567890` | ✅ |
| `CLOUDINARY_API_SECRET` | API Secret | `seu_api_secret` | ✅ |

### E-mail

| Variável | Descrição | Exemplo | Obrigatório |
|----------|-----------|---------|-----------|
| `MAIL_HOST` | Servidor SMTP | `smtp.gmail.com` | ✅ |
| `MAIL_PORT` | Porta SMTP | `587` | ❌ |
| `MAIL_USERNAME` | Usuário e-mail | `seu@email.com` | ✅ |
| `MAIL_PASSWORD` | Senha de app | `app_specific_password` | ✅ |
| `MAIL_FROM` | Endereço remetente | `noreply@codex.com` | ✅ |

### CORS

| Variável | Descrição | Exemplo | Obrigatório |
|----------|-----------|---------|-----------|
| `CORS_ALLOWED_ORIGINS` | Origens permitidas | `http://localhost:4200,https://app.vercel.app` | ❌ |

### Aplicação

| Variável | Descrição | Exemplo | Obrigatório |
|----------|-----------|---------|-----------|
| `ASPNETCORE_ENVIRONMENT` | Ambiente | `Development`, `Homolog`, `Production` | ❌ |
| `ASPNETCORE_URLS` | URLs de escuta | `http://localhost:5000` | ❌ |

---

## 🚢 Build e Deploy

### Build Local

```bash
# Debug
dotnet build

# Release
dotnet build --configuration Release -o ./build
```

### Publicar

```bash
# Desenvolvimento
dotnet publish -c Debug -o ./publish/debug

# Produção
dotnet publish -c Release -o ./publish/release
```

### Build Docker

```bash
# Build da imagem
docker build -t codex-backend:latest .

# Executar container
docker run -p 5000:5000 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e JWT_SECRET="sua_chave" \
  codex-backend:latest
```

### Deploy em Render

O backend é deployado automaticamente no **Render** quando você faz push:

```bash
# Configuração manual (se necessário)
# Arquivo: render.yaml (raiz do repositório)

services:
  - type: web
    name: codex-backend
    env: dotnet
    buildCommand: dotnet publish -c Release
    startCommand: dotnet ./bin/Release/net9.0/publish/CodexAPI.dll
    envVars:
      - key: ASPNETCORE_ENVIRONMENT
        value: Production
```

**URLs:**
- Homolog: `https://codex-homolog.onrender.com`
- Produção: `https://codex.onrender.com`

---

## 📊 Monitoramento

### Health Check

```bash
GET /health

Response:
{
  "status": "Healthy"
}
```

### Logging

O backend usa **Serilog** para logging estruturado:

```csharp
// Configuração em Program.cs
builder.Host.UseSerilog((context, configuration) =>
    configuration
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs/app-.txt", 
            rollingInterval: RollingInterval.Day)
);
```

**Ver logs:**

```bash
# Tempo real
tail -f logs/app-*.txt

# Filtrar erros
grep "ERROR" logs/app-*.txt
```

---

## 🆘 Troubleshooting

### Erro: "Connection string not found"

**Causa:** Variável de ambiente não configurada

**Solução:**
```bash
# Verificar
echo $DATABASE_URL

# Definir
export DATABASE_URL="sua_string"

# Ou em appsettings.json
# Ou em user-secrets
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "sua_string"
```

### Erro: "JWT SecretKey not configured"

**Causa:** JWT_SECRET não definido

**Solução:**
```bash
export JWT_SECRET="seu_secreto_minimo_32_caracteres"
```

### Erro: "CORS policy: No 'Access-Control-Allow-Origin'"

**Causa:** Frontend não autorizado

**Solução:**
```json
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200", "https://seu-app.vercel.app"]
  }
}
```

### Erro: "Migrations pending"

**Causa:** Migrações não aplicadas

**Solução:**
```bash
dotnet ef database update
```

### Erro: "Port 5000 already in use"

**Causa:** Outra aplicação usando a porta

**Solução:**
```bash
# Usar outra porta
dotnet run --urls "http://localhost:5001"

# Ou liberar porta (Linux)
sudo lsof -i :5000
sudo kill -9 <PID>
```

### Erro ao conectar PostgreSQL/Neon

**Causa:** SSL ou credenciais incorretas

**Solução:**
```bash
# Verificar conexão
psql "postgresql://usuario:senha@host:5432/database?sslmode=require"

# Testar com psql
psql -h host -U usuario -d database
```

---

## 📐 Padrões e Convenções

### Naming Conventions

- **Controllers:** `NomeController` (ex: `AuthController`)
- **Services:** `INomeService` / `NomeService`
- **Repositories:** `INomeRepository` / `NomeRepository`
- **Models:** `Nome` (ex: `Usuario`)
- **DTOs:** `NomeDto` ou `NomeRequest`/`NomeResponse`

### Estrutura de DTO

```csharp
public class CreateUsuarioRequest
{
    [Required]
    public string Nome { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(8)]
    public string Senha { get; set; }
}

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Tipo { get; set; }
}
```

### Repository Pattern

```csharp
public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveChangesAsync();
}
```

### Dependency Injection

```csharp
// Em Program.cs
builder.Services
    .AddScoped<IAuthService, AuthService>()
    .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
    .AddAutoMapper(typeof(Program));
```

---

## 👥 Créditos

### Equipe Backend

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/seu-usuario">
        <img src="https://github.com/seu-usuario.png" width="80px" alt="Seu Nome"/>
        <br />
        <sub><b>Seu Nome</b></sub>
      </a>
      <br />
      <small>Backend Lead Developer</small>
    </td>
  </tr>
</table>

---

<div align="center">
  <p>Feito com 💜 pela equipe CODEX</p>
  <p><strong>Organize, compartilhe e evolua seu conhecimento técnico.</strong></p>
  <sub>© 2026 CODEX Backend. Todos os direitos reservados.</sub>
</div>

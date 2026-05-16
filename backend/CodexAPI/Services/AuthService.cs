using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodexAPI.DTOs;
using CodexAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodexAPI.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request);
    Task<UsuarioDto?> GetCurrentUserAsync(int usuarioId);
    Task<UsuarioDto?> UpdateProfileAsync(int usuarioId, UpdateProfileDto request);
    string GenerateToken(Usuario usuario);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IConfiguration configuration, IUsuarioRepository usuarioRepository, ILogger<AuthService> logger)
    {
        _configuration = configuration;
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        try
        {
            // Validar entrada
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
                return null;

            // Buscar usuário
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (usuario == null || !usuario.Ativo)
            {
                _logger.LogWarning($"Tentativa de login com email não encontrado ou inativo: {request.Email}");
                return null;
            }

            // Verificar senha
            if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            {
                _logger.LogWarning($"Senha incorreta para usuário: {request.Email}");
                return null;
            }

            // Gerar token
            var token = GenerateToken(usuario);
            var expiresAt = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:ExpirationHours"] ?? "24"));

            return new AuthResponseDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Avatar = usuario.Avatar,
                Token = token,
                TokenExpiresAt = expiresAt,
                Papel = usuario.Papel ?? "usuario",
                Cpf = usuario.Cpf,
                DataNascimento = usuario.DataNascimento?.ToString("yyyy-MM-dd"),
                Genero = usuario.Genero,
                Cep = usuario.Cep,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Complemento = usuario.Complemento,
                Bairro = usuario.Bairro,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return null;
        }
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            // Validar entrada
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
                return null;

            if (request.Senha != request.ConfirmarSenha)
                return null;

            if (request.Senha.Length < 6)
                return null;

            // Verificar se email já existe
            var usuarioExistente = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (usuarioExistente != null)
                return null;

            // Criar novo usuário
            var usuario = new Usuario
            {
                Email = request.Email,
                Nome = request.Nome,
                Sobrenome = request.Sobrenome,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                Avatar = request.Avatar ?? GenerateDefaultAvatar(request.Email),
                Papel = "usuario",
                Ativo = true
            };

            var usuarioCriado = await _usuarioRepository.AddAsync(usuario);
            await _usuarioRepository.SaveAsync();

            // Gerar token
            var token = GenerateToken(usuarioCriado);
            var expiresAt = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:ExpirationHours"] ?? "24"));

            _logger.LogInformation($"Novo usuário registrado: {request.Email}");

            return new AuthResponseDto
            {
                Id = usuarioCriado.Id,
                Email = usuarioCriado.Email,
                Nome = usuarioCriado.Nome,
                Sobrenome = usuarioCriado.Sobrenome,
                Avatar = usuarioCriado.Avatar,
                Token = token,
                TokenExpiresAt = expiresAt,
                Papel = usuarioCriado.Papel ?? "usuario"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário");
            return null;
        }
    }

    public async Task<UsuarioDto?> GetCurrentUserAsync(int usuarioId)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                return null;

            return new UsuarioDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Avatar = usuario.Avatar,
                Ativo = usuario.Ativo,
                DataCriacao = usuario.DataCriacao,
                Papel = usuario.Papel ?? "usuario",
                Cpf = usuario.Cpf,
                DataNascimento = usuario.DataNascimento?.ToString("yyyy-MM-dd"),
                Genero = usuario.Genero,
                Cep = usuario.Cep,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Complemento = usuario.Complemento,
                Bairro = usuario.Bairro,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário atual");
            return null;
        }
    }

    public string GenerateToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, $"{usuario.Nome} {usuario.Sobrenome}"),
            new Claim(ClaimTypes.Role, usuario.Papel ?? "usuario")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:ExpirationHours"] ?? "24")),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<UsuarioDto?> UpdateProfileAsync(int usuarioId, UpdateProfileDto request)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return null;

            if (!string.IsNullOrWhiteSpace(request.Nome))
                usuario.Nome = request.Nome;
            if (!string.IsNullOrWhiteSpace(request.Sobrenome))
                usuario.Sobrenome = request.Sobrenome;
            if (request.Avatar != null)
                usuario.Avatar = request.Avatar;

            // CPF e DataNascimento: só pode setar uma vez (imutável depois)
            if (!string.IsNullOrWhiteSpace(request.Cpf) && string.IsNullOrWhiteSpace(usuario.Cpf))
                usuario.Cpf = request.Cpf;
            if (!string.IsNullOrWhiteSpace(request.DataNascimento) && usuario.DataNascimento == null)
            {
                if (DateTime.TryParse(request.DataNascimento, out var dt))
                    usuario.DataNascimento = dt;
            }

            // Genero e endereço: sempre editáveis
            if (request.Genero != null)
                usuario.Genero = request.Genero;
            if (request.Cep != null)
                usuario.Cep = request.Cep;
            if (request.Rua != null)
                usuario.Rua = request.Rua;
            if (request.Numero != null)
                usuario.Numero = request.Numero;
            if (request.Complemento != null)
                usuario.Complemento = request.Complemento;
            if (request.Bairro != null)
                usuario.Bairro = request.Bairro;
            if (request.Cidade != null)
                usuario.Cidade = request.Cidade;
            if (request.Estado != null)
                usuario.Estado = request.Estado;

            usuario.DataAtualizacao = DateTime.UtcNow;
            _usuarioRepository.Update(usuario);
            await _usuarioRepository.SaveAsync();

            return new UsuarioDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                Avatar = usuario.Avatar,
                Ativo = usuario.Ativo,
                DataCriacao = usuario.DataCriacao,
                Papel = usuario.Papel ?? "usuario",
                Cpf = usuario.Cpf,
                DataNascimento = usuario.DataNascimento?.ToString("yyyy-MM-dd"),
                Genero = usuario.Genero,
                Cep = usuario.Cep,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Complemento = usuario.Complemento,
                Bairro = usuario.Bairro,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar perfil");
            return null;
        }
    }

    private string GenerateDefaultAvatar(string email)
    {
        return $"https://api.dicebear.com/7.x/avataaars/svg?seed={Uri.EscapeDataString(email)}";
    }
}

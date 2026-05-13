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
                Papel = usuario.Papel ?? "usuario"
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
                Papel = usuario.Papel ?? "usuario"
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

    private string GenerateDefaultAvatar(string email)
    {
        return $"https://api.dicebear.com/7.x/avataaars/svg?seed={Uri.EscapeDataString(email)}";
    }
}

using CodexAPI.DTOs;
using CodexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodexAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login de usuário
    /// </summary>
    /// <param name="request">Email e senha</param>
    /// <returns>Token JWT e dados do usuário</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Sucesso = false,
                Mensagem = "Dados inválidos",
                Erros = errors
            });
        }

        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            return Unauthorized(new ApiResponse<AuthResponseDto>
            {
                Sucesso = false,
                Mensagem = "Email ou senha inválidos",
                Erros = new[] { "Falha na autenticação" }
            });
        }

        _logger.LogInformation($"Usuário {request.Email} realizou login com sucesso");

        return Ok(new ApiResponse<AuthResponseDto>
        {
            Sucesso = true,
            Mensagem = "Login realizado com sucesso",
            Dados = result
        });
    }

    /// <summary>
    /// Registro de novo usuário
    /// </summary>
    /// <param name="request">Dados do novo usuário</param>
    /// <returns>Token JWT e dados do usuário criado</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Sucesso = false,
                Mensagem = "Dados inválidos",
                Erros = errors
            });
        }

        // Validações específicas
        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
        {
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Sucesso = false,
                Mensagem = "Email inválido",
                Erros = new[] { "Email deve ser válido" }
            });
        }

        if (request.Senha.Length < 6)
        {
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Sucesso = false,
                Mensagem = "Senha muito curta",
                Erros = new[] { "Senha deve ter pelo menos 6 caracteres" }
            });
        }

        if (request.Senha != request.ConfirmarSenha)
        {
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Sucesso = false,
                Mensagem = "Senhas não conferem",
                Erros = new[] { "As senhas informadas não são iguais" }
            });
        }

        var result = await _authService.RegisterAsync(request);

        if (result == null)
        {
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Sucesso = false,
                Mensagem = "Falha ao registrar usuário",
                Erros = new[] { "Email já existe ou validação falhou" }
            });
        }

        _logger.LogInformation($"Novo usuário registrado: {request.Email}");

        return CreatedAtAction(nameof(Me), new { }, new ApiResponse<AuthResponseDto>
        {
            Sucesso = true,
            Mensagem = "Usuário registrado com sucesso",
            Dados = result
        });
    }

    /// <summary>
    /// Obter dados do usuário autenticado
    /// </summary>
    /// <returns>Dados do usuário atual</returns>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> Me()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized(new ApiResponse<UsuarioDto>
            {
                Sucesso = false,
                Mensagem = "Usuário não autenticado",
            });
        }

        var user = await _authService.GetCurrentUserAsync(userId);
        if (user == null)
        {
            return NotFound(new ApiResponse<UsuarioDto>
            {
                Sucesso = false,
                Mensagem = "Usuário não encontrado",
            });
        }

        return Ok(new ApiResponse<UsuarioDto>
        {
            Sucesso = true,
            Dados = user
        });
    }

    /// <summary>
    /// Verificar se token é válido
    /// </summary>
    /// <returns>Status do token</returns>
    [HttpGet("verify")]
    [Authorize]
    public ActionResult<ApiResponse<object>> VerifyToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var emailClaim = User.FindFirst(ClaimTypes.Email);

        return Ok(new ApiResponse<object>
        {
            Sucesso = true,
            Mensagem = "Token válido",
            Dados = new
            {
                userId = userIdClaim?.Value,
                email = emailClaim?.Value,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            }
        });
    }

    /// <summary>
    /// Atualizar perfil do usuário autenticado
    /// </summary>
    [HttpPut("perfil")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UsuarioDto>>> UpdateProfile([FromBody] UpdateProfileDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized(new ApiResponse<UsuarioDto>
            {
                Sucesso = false,
                Mensagem = "Usuário não autenticado"
            });
        }

        var result = await _authService.UpdateProfileAsync(userId, request);
        if (result == null)
        {
            return NotFound(new ApiResponse<UsuarioDto>
            {
                Sucesso = false,
                Mensagem = "Usuário não encontrado"
            });
        }

        return Ok(new ApiResponse<UsuarioDto>
        {
            Sucesso = true,
            Mensagem = "Perfil atualizado com sucesso",
            Dados = result
        });
    }
}

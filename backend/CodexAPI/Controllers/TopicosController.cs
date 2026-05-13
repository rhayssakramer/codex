using CodexAPI.DTOs;
using CodexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodexAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TopicosController : ControllerBase
{
    private readonly ITopicoRepository _topicoRepository;
    private readonly IProgressoTopicoRepository _progressoRepository;
    private readonly ILogger<TopicosController> _logger;

    public TopicosController(
        ITopicoRepository topicoRepository,
        IProgressoTopicoRepository progressoRepository,
        ILogger<TopicosController> logger)
    {
        _topicoRepository = topicoRepository;
        _progressoRepository = progressoRepository;
        _logger = logger;
    }

    /// <summary>
    /// Listar tópicos de uma disciplina
    /// </summary>
    [HttpGet("disciplina/{disciplinaId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<TopicoDto>>>> GetByDisciplinaId(int disciplinaId)
    {
        try
        {
            var topicos = await _topicoRepository.GetByDisciplinaIdAsync(disciplinaId);
            var topicosDto = topicos.Select(t => new TopicoDto
            {
                Id = t.Id,
                DisciplinaId = t.DisciplinaId,
                Titulo = t.Titulo,
                Conteudo = t.Conteudo,
                CodigoExemplo = t.CodigoExemplo,
                VideoUrl = t.VideoUrl,
                Ordem = t.Ordem,
                Dificuldade = t.Dificuldade,
                Ativo = t.Ativo,
                Concluido = false,
                ProgressoPercentual = 0
            }).ToList();

            return Ok(new ApiResponse<List<TopicoDto>>
            {
                Sucesso = true,
                Dados = topicosDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tópicos");
            return StatusCode(500, new ApiResponse<List<TopicoDto>>
            {
                Sucesso = false,
                Mensagem = "Erro ao listar tópicos"
            });
        }
    }

    /// <summary>
    /// Obter tópico por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<TopicoDto>>> GetById(int id)
    {
        try
        {
            var topico = await _topicoRepository.GetByIdAsync(id);
            if (topico == null)
            {
                return NotFound(new ApiResponse<TopicoDto>
                {
                    Sucesso = false,
                    Mensagem = "Tópico não encontrado"
                });
            }

            // Buscar progresso do usuário se autenticado
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var progresso = new ProgressoTopicoDto();

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                var progressoUsuario = await _progressoRepository.GetByUsuarioAndTopicoAsync(userId, id);
                if (progressoUsuario != null)
                {
                    progresso.Concluido = progressoUsuario.Concluido;
                    progresso.Progresso = progressoUsuario.Progresso ?? 0;
                }
            }

            var topicoDto = new TopicoDto
            {
                Id = topico.Id,
                DisciplinaId = topico.DisciplinaId,
                Titulo = topico.Titulo,
                Conteudo = topico.Conteudo,
                CodigoExemplo = topico.CodigoExemplo,
                VideoUrl = topico.VideoUrl,
                Ordem = topico.Ordem,
                Dificuldade = topico.Dificuldade,
                Ativo = topico.Ativo,
                Concluido = progresso.Concluido,
                ProgressoPercentual = progresso.Progresso
            };

            return Ok(new ApiResponse<TopicoDto>
            {
                Sucesso = true,
                Dados = topicoDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter tópico");
            return StatusCode(500, new ApiResponse<TopicoDto>
            {
                Sucesso = false,
                Mensagem = "Erro ao obter tópico"
            });
        }
    }

    /// <summary>
    /// Criar tópico (admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateTopicoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
                return BadRequest(new ApiResponse<object>
                {
                    Sucesso = false,
                    Mensagem = "Dados inválidos",
                    Erros = errors
                });
            }

            var topico = new Models.Topico
            {
                DisciplinaId = dto.DisciplinaId,
                Titulo = dto.Titulo,
                Conteudo = dto.Conteudo,
                CodigoExemplo = dto.CodigoExemplo,
                VideoUrl = dto.VideoUrl,
                Ordem = dto.Ordem,
                Dificuldade = dto.Dificuldade
            };

            await _topicoRepository.AddAsync(topico);
            await _topicoRepository.SaveAsync();

            _logger.LogInformation($"Tópico criado: {topico.Titulo}");

            return CreatedAtAction(nameof(GetById), new { id = topico.Id }, new ApiResponse<object>
            {
                Sucesso = true,
                Mensagem = "Tópico criado com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar tópico");
            return StatusCode(500, new ApiResponse<object>
            {
                Sucesso = false,
                Mensagem = "Erro ao criar tópico"
            });
        }
    }

    /// <summary>
    /// Atualizar progresso de um tópico
    /// </summary>
    [HttpPost("{id}/progresso")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ProgressoTopicoDto>>> UpdateProgresso(int id, [FromBody] UpdateProgressoDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse<ProgressoTopicoDto>
                {
                    Sucesso = false,
                    Mensagem = "Usuário não autenticado"
                });
            }

            var topico = await _topicoRepository.GetByIdAsync(id);
            if (topico == null)
            {
                return NotFound(new ApiResponse<ProgressoTopicoDto>
                {
                    Sucesso = false,
                    Mensagem = "Tópico não encontrado"
                });
            }

            var progresso = await _progressoRepository.GetByUsuarioAndTopicoAsync(userId, id);

            if (progresso == null)
            {
                progresso = new Models.ProgressoTopico
                {
                    UsuarioId = userId,
                    TopicoId = id,
                    Concluido = dto.Concluido ?? false,
                    Progresso = dto.Progresso ?? 0
                };

                await _progressoRepository.AddAsync(progresso);
            }
            else
            {
                if (dto.Concluido.HasValue)
                    progresso.Concluido = dto.Concluido.Value;

                if (dto.Progresso.HasValue)
                    progresso.Progresso = Math.Min(100, Math.Max(0, dto.Progresso.Value));

                _progressoRepository.Update(progresso);
            }

            await _progressoRepository.SaveAsync();

            _logger.LogInformation($"Progresso atualizado para usuário {userId} no tópico {id}");

            return Ok(new ApiResponse<ProgressoTopicoDto>
            {
                Sucesso = true,
                Mensagem = "Progresso atualizado com sucesso",
                Dados = new ProgressoTopicoDto
                {
                    Id = progresso.Id,
                    TopicoId = progresso.TopicoId,
                    Concluido = progresso.Concluido,
                    Progresso = progresso.Progresso ?? 0,
                    DataInicio = progresso.DataInicio,
                    DataConclusao = progresso.DataConclusao
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar progresso");
            return StatusCode(500, new ApiResponse<ProgressoTopicoDto>
            {
                Sucesso = false,
                Mensagem = "Erro ao atualizar progresso"
            });
        }
    }

    /// <summary>
    /// Obter progresso do usuário em um tópico
    /// </summary>
    [HttpGet("{id}/meu-progresso")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ProgressoTopicoDto>>> GetMeuProgresso(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse<ProgressoTopicoDto>
                {
                    Sucesso = false,
                    Mensagem = "Usuário não autenticado"
                });
            }

            var progresso = await _progressoRepository.GetByUsuarioAndTopicoAsync(userId, id);

            if (progresso == null)
            {
                return Ok(new ApiResponse<ProgressoTopicoDto>
                {
                    Sucesso = true,
                    Dados = new ProgressoTopicoDto
                    {
                        TopicoId = id,
                        Concluido = false,
                        Progresso = 0
                    }
                });
            }

            return Ok(new ApiResponse<ProgressoTopicoDto>
            {
                Sucesso = true,
                Dados = new ProgressoTopicoDto
                {
                    Id = progresso.Id,
                    TopicoId = progresso.TopicoId,
                    Concluido = progresso.Concluido,
                    Progresso = progresso.Progresso ?? 0,
                    DataInicio = progresso.DataInicio,
                    DataConclusao = progresso.DataConclusao
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter progresso");
            return StatusCode(500, new ApiResponse<ProgressoTopicoDto>
            {
                Sucesso = false,
                Mensagem = "Erro ao obter progresso"
            });
        }
    }
}

using CodexAPI.DTOs;
using CodexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodexAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AreasController : ControllerBase
{
    private readonly IAreaRepository _areaRepository;
    private readonly ILogger<AreasController> _logger;

    public AreasController(IAreaRepository areaRepository, ILogger<AreasController> logger)
    {
        _areaRepository = areaRepository;
        _logger = logger;
    }

    /// <summary>
    /// Listar todas as áreas com disciplinas
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<AreaDto>>>> GetAll()
    {
        try
        {
            var areas = await _areaRepository.GetAllWithDisciplinasAsync();
            var areasDto = areas.Select(a => new AreaDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Descricao = a.Descricao,
                Icone = a.Icone,
                Ordem = a.Ordem,
                Ativo = a.Ativo,
                Disciplinas = a.Disciplinas?.Select(d => new DisciplinaDto
                {
                    Id = d.Id,
                    AreaId = d.AreaId,
                    Nome = d.Nome,
                    Descricao = d.Descricao,
                    Imagem = d.Imagem,
                    Ordem = d.Ordem,
                    Ativo = d.Ativo,
                    TotalTopicos = d.Topicos?.Count ?? 0,
                    TopicosConcluidos = 0 // Será preenchido com progresso do usuário se necessário
                }).ToList() ?? []
            }).ToList();

            return Ok(new ApiResponse<List<AreaDto>>
            {
                Sucesso = true,
                Dados = areasDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar áreas");
            return StatusCode(500, new ApiResponse<List<AreaDto>>
            {
                Sucesso = false,
                Mensagem = "Erro ao listar áreas",
                Erros = new[] { ex.Message }
            });
        }
    }

    /// <summary>
    /// Obter área por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AreaDto>>> GetById(int id)
    {
        try
        {
            var area = await _areaRepository.GetByIdAsync(id);
            if (area == null)
            {
                return NotFound(new ApiResponse<AreaDto>
                {
                    Sucesso = false,
                    Mensagem = "Área não encontrada"
                });
            }

            var areaDto = new AreaDto
            {
                Id = area.Id,
                Nome = area.Nome,
                Descricao = area.Descricao,
                Icone = area.Icone,
                Ordem = area.Ordem,
                Ativo = area.Ativo,
                Disciplinas = area.Disciplinas?.Select(d => new DisciplinaDto
                {
                    Id = d.Id,
                    AreaId = d.AreaId,
                    Nome = d.Nome,
                    Descricao = d.Descricao,
                    Imagem = d.Imagem,
                    Ordem = d.Ordem,
                    Ativo = d.Ativo,
                    TotalTopicos = d.Topicos?.Count ?? 0,
                    TopicosConcluidos = 0
                }).ToList() ?? []
            };

            return Ok(new ApiResponse<AreaDto>
            {
                Sucesso = true,
                Dados = areaDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter área");
            return StatusCode(500, new ApiResponse<AreaDto>
            {
                Sucesso = false,
                Mensagem = "Erro ao obter área"
            });
        }
    }

    /// <summary>
    /// Criar nova área (admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<AreaDto>>> Create([FromBody] CreateAreaDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
                return BadRequest(new ApiResponse<AreaDto>
                {
                    Sucesso = false,
                    Mensagem = "Dados inválidos",
                    Erros = errors
                });
            }

            var area = new Models.Area
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Icone = dto.Icone,
                Ordem = dto.Ordem
            };

            await _areaRepository.AddAsync(area);
            await _areaRepository.SaveAsync();

            _logger.LogInformation($"Área criada: {area.Nome}");

            return CreatedAtAction(nameof(GetById), new { id = area.Id }, new ApiResponse<AreaDto>
            {
                Sucesso = true,
                Mensagem = "Área criada com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar área");
            return StatusCode(500, new ApiResponse<AreaDto>
            {
                Sucesso = false,
                Mensagem = "Erro ao criar área"
            });
        }
    }
}

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DisciplinasController : ControllerBase
{
    private readonly IDisciplinaRepository _disciplinaRepository;
    private readonly IAreaRepository _areaRepository;
    private readonly ILogger<DisciplinasController> _logger;

    public DisciplinasController(IDisciplinaRepository disciplinaRepository, IAreaRepository areaRepository, ILogger<DisciplinasController> logger)
    {
        _disciplinaRepository = disciplinaRepository;
        _areaRepository = areaRepository;
        _logger = logger;
    }

    /// <summary>
    /// Listar disciplinas por área
    /// </summary>
    [HttpGet("area/{areaId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<DisciplinaDto>>>> GetByAreaId(int areaId)
    {
        try
        {
            var disciplinas = await _disciplinaRepository.GetByAreaIdAsync(areaId);
            var disciplinasDto = disciplinas.Select(d => new DisciplinaDto
            {
                Id = d.Id,
                AreaId = d.AreaId,
                Nome = d.Nome,
                Descricao = d.Descricao,
                Imagem = d.Imagem,
                Ordem = d.Ordem,
                Ativo = d.Ativo,
                TotalTopicos = d.Topicos?.Count ?? 0,
                TopicosConcluidos = 0
            }).ToList();

            return Ok(new ApiResponse<List<DisciplinaDto>>
            {
                Sucesso = true,
                Dados = disciplinasDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar disciplinas");
            return StatusCode(500, new ApiResponse<List<DisciplinaDto>>
            {
                Sucesso = false,
                Mensagem = "Erro ao listar disciplinas"
            });
        }
    }

    /// <summary>
    /// Obter disciplina por ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<DisciplinaDto>>> GetById(int id)
    {
        try
        {
            var disciplina = await _disciplinaRepository.GetByIdAsync(id);
            if (disciplina == null)
            {
                return NotFound(new ApiResponse<DisciplinaDto>
                {
                    Sucesso = false,
                    Mensagem = "Disciplina não encontrada"
                });
            }

            var disciplinaDto = new DisciplinaDto
            {
                Id = disciplina.Id,
                AreaId = disciplina.AreaId,
                Nome = disciplina.Nome,
                Descricao = disciplina.Descricao,
                Imagem = disciplina.Imagem,
                Ordem = disciplina.Ordem,
                Ativo = disciplina.Ativo,
                TotalTopicos = disciplina.Topicos?.Count ?? 0,
                TopicosConcluidos = 0
            };

            return Ok(new ApiResponse<DisciplinaDto>
            {
                Sucesso = true,
                Dados = disciplinaDto
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter disciplina");
            return StatusCode(500, new ApiResponse<DisciplinaDto>
            {
                Sucesso = false,
                Mensagem = "Erro ao obter disciplina"
            });
        }
    }

    /// <summary>
    /// Criar disciplina (admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] CreateDisciplinaDto dto)
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

            var disciplina = new Models.Disciplina
            {
                AreaId = dto.AreaId,
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Imagem = dto.Imagem,
                Ordem = dto.Ordem
            };

            await _disciplinaRepository.AddAsync(disciplina);
            await _disciplinaRepository.SaveAsync();

            _logger.LogInformation($"Disciplina criada: {disciplina.Nome}");

            return CreatedAtAction(nameof(GetById), new { id = disciplina.Id }, new ApiResponse<object>
            {
                Sucesso = true,
                Mensagem = "Disciplina criada com sucesso"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar disciplina");
            return StatusCode(500, new ApiResponse<object>
            {
                Sucesso = false,
                Mensagem = "Erro ao criar disciplina"
            });
        }
    }
}

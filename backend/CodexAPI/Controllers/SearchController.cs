using CodexAPI.DTOs;
using CodexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodexAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class SearchController : ControllerBase
{
    private readonly IAreaRepository _areaRepository;
    private readonly IDisciplinaRepository _disciplinaRepository;
    private readonly ITopicoRepository _topicoRepository;
    private readonly ILogger<SearchController> _logger;

    public SearchController(
        IAreaRepository areaRepository,
        IDisciplinaRepository disciplinaRepository,
        ITopicoRepository topicoRepository,
        ILogger<SearchController> logger)
    {
        _areaRepository = areaRepository;
        _disciplinaRepository = disciplinaRepository;
        _topicoRepository = topicoRepository;
        _logger = logger;
    }

    /// <summary>
    /// Buscar áreas, disciplinas e tópicos por termo de busca
    /// </summary>
    /// <param name="q">Termo de busca</param>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SearchResultDto>>>> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Ok(new ApiResponse<List<SearchResultDto>>
            {
                Sucesso = true,
                Dados = new List<SearchResultDto>()
            });
        }

        try
        {
            var query = q.ToLower().Trim();
            var resultados = new List<SearchResultDto>();

            _logger.LogInformation($"🔍 Buscando por: '{query}'");

            // Buscar áreas
            var areas = await _areaRepository.GetAllAsync();
            _logger.LogInformation($"📊 Total de áreas no banco: {areas.Count()}");
            
            var areasEncontradas = areas
                .Where(a => a.Ativo && (a.Nome.ToLower().Contains(query) || a.Descricao.ToLower().Contains(query)))
                .Select(a => new SearchResultDto
                {
                    Id = NormalizeAreaName(a.Nome),
                    Title = a.Nome,
                    Description = a.Descricao,
                    Type = "area",
                    Icon = "fa-folder"
                })
                .ToList();
            _logger.LogInformation($"✅ Áreas encontradas com '{query}': {areasEncontradas.Count}");
            resultados.AddRange(areasEncontradas);

            // Buscar disciplinas
            var disciplinas = await _disciplinaRepository.GetAllAsync();
            _logger.LogInformation($"📊 Total de disciplinas no banco: {disciplinas.Count()}");
            
            var disciplinasEncontradas = disciplinas
                .Where(d => d.Ativo && (d.Nome.ToLower().Contains(query) || d.Descricao.ToLower().Contains(query)))
                .Select(d => {
                    var areaSlug = NormalizeAreaName(d.Area?.Nome ?? "");
                    return new SearchResultDto
                    {
                        Id = d.Id.ToString(),
                        Title = d.Nome,
                        Description = d.Descricao,
                        Type = "disciplina",
                        Area = areaSlug,
                        Icon = "fa-book"
                    };
                })
                .ToList();
            _logger.LogInformation($"✅ Disciplinas encontradas com '{query}': {disciplinasEncontradas.Count}");
            resultados.AddRange(disciplinasEncontradas);

            // Buscar tópicos - MAIS IMPORTANTE
            var topicos = await _topicoRepository.GetAllAsync();
            
            _logger.LogInformation($"📊 Total de tópicos no banco: {topicos.Count()}");
            _logger.LogInformation($"📊 Tópicos ativos: {topicos.Count(t => t.Ativo)}");
            
            // Log cada tópico para debug
            foreach (var topico in topicos)
            {
                _logger.LogInformation($"  - Tópico: {topico.Titulo} (ID: {topico.Id}, Ativo: {topico.Ativo}, DisciplinaId: {topico.DisciplinaId})");
            }
            
            var topicosEncontrados = topicos
                .Where(t => t.Ativo && t.Titulo.ToLower().Contains(query))
                .Select(t => {
                    // Buscar a disciplina correta
                    var disciplina = disciplinas.FirstOrDefault(d => d.Id == t.DisciplinaId);
                    // Converter nome da área para slug (fundamentos, frontend, backend, devops, certificacoes)
                    var areaSlug = NormalizeAreaName(disciplina?.Area?.Nome ?? "");
                    return new SearchResultDto
                    {
                        Id = t.Id.ToString(),
                        Title = t.Titulo,
                        Description = t.Conteudo?.Length > 100 ? t.Conteudo.Substring(0, 100) + "..." : t.Conteudo,
                        Type = "topico",
                        Area = areaSlug,
                        DisciplinaId = t.DisciplinaId.ToString(),
                        Icon = "fa-file"
                    };
                })
                .ToList();
            _logger.LogInformation($"✅ Tópicos encontrados com '{query}': {topicosEncontrados.Count}");
            resultados.AddRange(topicosEncontrados);
            
            _logger.LogInformation($"📈 Total de resultados finais: {resultados.Count}");

            return Ok(new ApiResponse<List<SearchResultDto>>
            {
                Sucesso = true,
                Dados = resultados.OrderBy(r => r.Title).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao buscar");
            return StatusCode(500, new ApiResponse<List<SearchResultDto>>
            {
                Sucesso = false,
                Mensagem = "Erro ao realizar busca",
                Erros = new[] { ex.Message }
            });
        }
    }

    /// <summary>
    /// Normaliza o nome da área para o slug esperado pelas rotas
    /// </summary>
    private string NormalizeAreaName(string areaName)
    {
        return areaName.ToLower()
            .Replace("ã", "a")
            .Replace("ç", "c")
            .Replace(" ", "")
            .Trim();
    }
}

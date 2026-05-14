using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodexAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UploadsController : ControllerBase
{
    private readonly ILogger<UploadsController> _logger;
    private readonly string _uploadPath;

    public UploadsController(ILogger<UploadsController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _uploadPath = Path.Combine(env.WebRootPath, "uploads");
    }

    [HttpPost("image")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Nenhum arquivo foi enviado" });

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        
        if (!allowedExtensions.Contains(fileExtension))
            return BadRequest(new { message = "Apenas arquivos de imagem são permitidos" });

        const long maxFileSize = 5 * 1024 * 1024;
        if (file.Length > maxFileSize)
            return BadRequest(new { message = "O arquivo não pode exceder 5MB" });

        try
        {
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation($"Arquivo '{uniqueFileName}' foi enviado com sucesso");

            return Ok(new
            {
                success = true,
                url = $"/uploads/{uniqueFileName}",
                filename = uniqueFileName,
                size = file.Length
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao fazer upload: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao fazer upload do arquivo" });
        }
    }

    [HttpDelete("image/{filename}")]
    [AllowAnonymous]
    public ActionResult DeleteImage(string filename)
    {
        if (filename.Contains("..") || filename.Contains("/") || filename.Contains("\\"))
            return BadRequest(new { message = "Nome de arquivo inválido" });

        try
        {
            var filePath = Path.Combine(_uploadPath, filename);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "Arquivo não encontrado" });

            System.IO.File.Delete(filePath);
            _logger.LogInformation($"Arquivo '{filename}' foi deletado");

            return Ok(new { message = "Imagem deletada com sucesso!" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao deletar imagem: {ex.Message}");
            return StatusCode(500, new { message = "Erro ao deletar arquivo" });
        }
    }
}

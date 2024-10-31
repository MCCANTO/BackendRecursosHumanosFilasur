using BackEndRecursosHumanosFilasur.Data;
using BackEndRecursosHumanosFilasur.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEndRecursosHumanosFilasur.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanFileController : _BaseController
{
    public LoanFileController(BaseContext context) : base(context) { }
    
    [HttpGet]
    [Route("GetAllLoanFileRequest/{usercode}/{id_prestamo}")]
    public async Task<ActionResult<object>> GetAllLoanRequest([FromRoute] string usercode, int id_prestamo)
    {
        try
        {
            var result = await new LoanFileService(_context).GetAllLoanFileRequest(usercode, id_prestamo);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
    
    [HttpPost("upload/{p_usercode}/{p_id_prestamo}")]
    public async Task<IActionResult> UploadFiles([FromRoute] string p_usercode, int p_id_prestamo, [FromForm] List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest("No files were uploaded.");
        }
        
        var contador = 1;
        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var directoryPath = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, p_usercode, p_id_prestamo.ToString());
                var filePath = Path.Combine(directoryPath, contador + "__" + file.FileName);
            
                // Crear los directorios si no existen
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Guardar los documentos en el servidor
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            
                // Guardar los nombres de los documentos
                int saveInt = await new LoanFileService(_context).SaveAsync(p_id_prestamo, p_usercode, contador + "__" + file.FileName);

                if (saveInt == 0) return Conflict(new { msg = "Error: No hubo registro del archivo" });
            }

            contador++;
        }

        return Ok(new { UserCode = p_usercode, FileCount = files.Count });
    }
}
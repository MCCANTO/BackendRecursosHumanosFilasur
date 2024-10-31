using BackEndRecursosHumanosFilasur.Data;
using BackEndRecursosHumanosFilasur.Helpers;
using BackEndRecursosHumanosFilasur.Models.ContentBody;
using BackEndRecursosHumanosFilasur.Models.ContentResponse;
using BackEndRecursosHumanosFilasur.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEndRecursosHumanosFilasur.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanController : _BaseController
{
    public LoanController(BaseContext context) : base(context) { }
    
    [HttpGet]
    [Route("GetAllLoanRequest/{usercode}")]
    public async Task<ActionResult<object>> GetAllLoanRequest([FromRoute] string usercode)
    {
        try
        {
            string buscar = "";
            DateTime fechaInicio = DateTime.Today;
            DateTime fechaFin = DateTime.Today;
            var result = await new LoanService(_context).GetAllLoanRequest(fechaInicio, fechaFin, buscar, usercode);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetAllLoanEmployeeRequest/{empleado}")]
    public async Task<ActionResult<object>> GetAllLoanEmployeeRequest([FromRoute] string empleado)
    {
        try
        {
            var result = await new LoanService(_context).GetAllEmployee(empleado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

     [HttpPost]
     [Route("SaveLoanRequest")]
     public async Task<ActionResult<object>> Save([FromBody] LoanBody loanRequestResponse)
     {
         var result = await new LoanService(_context).RegistrarPrestamo(loanRequestResponse);
         return result;
     }


    [HttpGet]
    [Route("GetDocument")]
    public IActionResult GetDocument()
    {
        try
        {
            var directoryPath = AppConfig.Configuracion.CarpetaArchivos;
            var directoryInfo = new DirectoryInfo(directoryPath);

            var firstFile = directoryInfo.GetFiles().OrderBy(f => f.CreationTime).FirstOrDefault();
            if (firstFile == null)
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(firstFile.FullName);
            var mimeType = "application/octet-stream"; // Cambia esto según el tipo de archivo, si lo sabes

            return File(fileBytes, mimeType, firstFile.Name);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { msg = ex.Message });
        }
    }

    [HttpGet]
    [Route("GetReasonLoanRequest")]
    public async Task<ActionResult<object>> GetReasonLoan()
    {
        try
        {
            var result = await new LoanService(_context).GetReasonLoanAsync();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetAllLoanApplicationCreated/{usercode}")]
    public async Task<ActionResult<object>> GetAllLoanApplicationCreated([FromRoute] string usercode)
    {
        try
        {
            var result = await new LoanService(_context).GetLoanApplicationCreated(usercode);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpPut]
    [Route("UpdateEmployeeConfirmationStatus")]
    public async Task<ActionResult> UpdateEmployeeConfirmationState(LoanBody loanBody)
    {
        try
        {
            var provuser = await new LoanService(_context).UpdateLoanEmployeeConfirmation(loanBody);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpPut]
    [Route("UpdateSupervisorApprovalStatus")]
    public async Task<ActionResult> UpdateSupervisorState(LoanBody loanBody)
    {
        try
        {
            var provuser = await new LoanService(_context).UpdateLoanSupervisorApproval(loanBody);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpPut]
    [Route("UpdateManagerApprovalStatus")]
    public async Task<ActionResult> UpdateManagerApprovalState(LoanBody loanBody)
    {
        try
        {
            var provuser = await new LoanService(_context).UpdateLoanManagerApproval(loanBody);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpGet]
    [Route("GetApprovalEmployeeRequest")]
    public async Task<ActionResult<object>> GetAllApprovalRequest()
    {
        try
        {
            var result = await new LoanService(_context).GetApprovalRequestAsync();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetLoanDetailsRequest/{id_prestamo}")]
    public async Task<ActionResult<object>> GetLoanDetailRequest(int id_prestamo)
    {
        try
        {
            var result = await new LoanService(_context).GetLoanDetails(id_prestamo);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetLoanLimitAmountRequest/{empleado}")]
    public async Task<ActionResult<object>> GetLoanLimitAmountRequest([FromRoute] string empleado)
    {
        try
        {
            var result = await new LoanService(_context).GetLoanLimitAmount(empleado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
    
    [HttpGet("fileDownload/{p_user}/{p_id_prestamo}/{valor}")]
    public async Task<ActionResult<object>> FileDownload(string p_user, string p_id_prestamo, string valor)
    {
        try
        {
            string _file = Path.Combine(AppConfig.Configuracion.CarpetaArchivos, p_user, p_id_prestamo, valor ?? "");
            if (!System.IO.File.Exists(_file))
            {
                return NotFound();
            }

            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string? contentType;
            if (!provider.TryGetContentType(_file, out contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] filebyte = await System.IO.File.ReadAllBytesAsync(_file);

            if (filebyte.Length == 0)
            {
                return NotFound();
            }

            string base64 = Convert.ToBase64String(filebyte);

            return $"data:{contentType};base64,{base64}";

        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpPut]
    [Route("UpdateLoanRequestStatus")]
    public async Task<ActionResult> LoanRequestDeniedSupervisor(LoanBody loanBody)
    {
        try
        {
            var statusRequest = await new LoanService(_context).LoanRequestDeniedSupervisor(loanBody);
            if (statusRequest == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();
    }

    [HttpPut]
    [Route("UpdateLoanRequestStatusManager")]
    public async Task<ActionResult> LoanRequestDeniedManager(LoanBody loanBody)
    {
        try
        {
            var statusRequest = await new LoanService(_context).LoanRequestDeniedManager(loanBody);
            if (statusRequest == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();
    }

    [HttpGet]
    [Route("GetRequestStatus/{empleado}")]
    public async Task<ActionResult<object>> GetRequestState([FromRoute] string empleado)
    {
        try
        {
            var result = await new LoanService(_context).GetRequestState(empleado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }


    [HttpGet]
    [Route("GetRequestAllowedStatus/{empleado}")]
    public async Task<ActionResult<object>> GetRequestAllowedState([FromRoute] string empleado)
    {
        try
        {
            var result = await new LoanService(_context).GetRequestAllowedState(empleado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetRequestAsyncAll")]
    public async Task<ActionResult<object>> GetRequestAsyncAll()
    {
        try
        {
            var result = await new LoanService(_context).GetReasonLoanAsyncAll();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

}
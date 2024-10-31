using BackEndRecursosHumanosFilasur.Data;
using BackEndRecursosHumanosFilasur.Models.ContentBody;
using BackEndRecursosHumanosFilasur.Models.ContentResponse;
using BackEndRecursosHumanosFilasur.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEndRecursosHumanosFilasur.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdvanceSalaryController : _BaseController
{
    public AdvanceSalaryController(BaseContext context) : base(context) { }

    [HttpGet]
    [Route("GetAllAdvancedSalaryEv/{usercode}")]
    public async Task<ActionResult<object>> GetAllAdvancedSalary([FromRoute] string usercode)
    {
        try
        {
            string buscar = "";
            DateTime fechaInicio = DateTime.Today;
            DateTime fechaFin = DateTime.Today;

            var result = await new AdvanceSalaryService(_context).GetAllAdvancedSalaryEv(usercode);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetAllAdvancedSalaryRequest/{usercode}")]
    public async Task<ActionResult<object>> GetAllAdvancedSalaryRequest([FromRoute] string usercode)
    {
        try
        {
            string buscar = "";
            DateTime fechaInicio = DateTime.Today;
            DateTime fechaFin = DateTime.Today;

            var result = await new AdvanceSalaryService(_context).GetAllAdvancedSalaryRequest(usercode);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [Route("SaveSalaryRequest")]
    public async Task<ActionResult<object>> Save([FromBody] AdvanceSalaryBody advanceSalary)
    {
        var result = await new AdvanceSalaryService(_context).SaveAdvanceSalary(advanceSalary);
        return result;
    }

    [HttpGet]
    [Route("GetSalaryAdvanceAccount/{empleado}")]
    public async Task<ActionResult<object>> GetAllLoanEmployeeRequest([FromRoute] string empleado)
    {
        try
        {
            var result = await new AdvanceSalaryService(_context).GetSalaryAdvanceAccountEmployee(empleado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetAdvacendReasonRequest")]
    public async Task<ActionResult<object>> GetAdvacendReasonRequest()
    {
        try
        {
            var result = await new AdvanceSalaryService(_context).GetAdvanceReasonAsync();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetAdvacendRequestSupervisor")]
    public async Task<ActionResult<object>> GetAdvacendRequestSupervisor()
    {
        try
        {
            var result = await new AdvanceSalaryService(_context).GetAdvanceSalarySupervisor();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetAdvacendRequestManager")]
    public async Task<ActionResult<object>> GetAdvacendRequestManager()
    {
        try
        {
            var result = await new AdvanceSalaryService(_context).GetAdvanceSalaryManager();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpPut]
    [Route("UpdateAdvanceSalarySupervisorApprovalStatus")]
    public async Task<ActionResult> UpdateSupervisorState(AdvanceSalaryBody advance)
    {
        try
        {
            var provuser = await new AdvanceSalaryService(_context).UpdateAdvanceSalarySupervisorApproval(advance);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpPut]
    [Route("UpdateAdvanceSalaryManagerApprovalStatus")]
    public async Task<ActionResult> UpdateManagerApprovalState(AdvanceSalaryBody advance)
    {
        try
        {
            var provuser = await new AdvanceSalaryService(_context).UpdateAdvanceSalaryManagerApproval(advance);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpPut]
    [Route("UpdateAdvanceSalaryEmployeeApprovalStatus")]
    public async Task<ActionResult> UpdateEmployeeApprovalState(AdvanceSalaryBody advance)
    {
        try
        {
            var provuser = await new AdvanceSalaryService(_context).UpdateAdvanceSalaryEmployeeApproval(advance);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpGet]
    [Route("GetLimitAdvanceRequest/{empleado}")]
    public async Task<ActionResult<object>> GetLimitAdvanceRequest([FromRoute] string empleado)
    {
        try
        {
            var result = await new AdvanceSalaryService(_context).GetLimitAdvance(empleado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpPut]
    [Route("UpdateAdvanceSalaryStatus")]
    public async Task<ActionResult> UpdateAdvanceSalaryRequestStatus(AdvanceSalaryBody advance)
    {
        try
        {
            var provuser = await new AdvanceSalaryService(_context).AdvanceRequestDeniedSupervisor(advance);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpPut]
    [Route("UpdateAdvanceSalaryStatusManager")]
    public async Task<ActionResult> AdvanceRequestDeniedManager(AdvanceSalaryBody advance)
    {
        try
        {
            var provuser = await new AdvanceSalaryService(_context).AdvanceRequestDeniedManager(advance);
            if (provuser == null) return NotFound();
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }
        return NoContent();

    }

    [HttpGet]
    [Route("GetAdvanceStatusRequest/{empleado}")]
    public async Task<ActionResult<object>> GetAdvanceStatusRequest([FromRoute] string empleado)
    {
        try
        {
            var result = await new AdvanceSalaryService(_context).GetSalaryAdvanceStatusRequest(empleado);
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("GetAdvanceRequestAsyncAll")]
    public async Task<ActionResult<object>> GetAdvanceRequestAsyncAll()
    {
        try
        {
            var result = await new AdvanceSalaryService(_context).GetSalaryAdvanceAsyncAll();
            return result;
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }
}
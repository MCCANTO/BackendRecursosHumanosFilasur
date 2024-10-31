using BackEndRecursosHumanosFilasur.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackEndRecursosHumanosFilasur.Controllers;

public class _BaseController : ControllerBase
{
    protected readonly BaseContext _context = null!;
    public _BaseController(BaseContext context) => _context = context;
}
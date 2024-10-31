using BackEndRecursosHumanosFilasur.Data;

namespace BackEndRecursosHumanosFilasur.Services;

public class _BaseService 
{
    protected readonly BaseContext _context = null!;
    
    protected _BaseService(BaseContext context) => _context = context;
    
}
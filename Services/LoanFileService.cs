using BackEndRecursosHumanosFilasur.Data;
using BackEndRecursosHumanosFilasur.Models.ContentResponse;
using Microsoft.EntityFrameworkCore;

namespace BackEndRecursosHumanosFilasur.Services;

public class LoanFileService : _BaseService
{
    
    public LoanFileService(BaseContext context) : base(context) { }
    
    public async Task<object> GetAllLoanFileRequest(string usercode, int id_prestamo)
    {
        try
        {
            var result = await _context.LoanFileResponse.FromSqlInterpolated($"exec filasur.xtus_rh_archivos_prestamo {usercode}, {id_prestamo}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    
    public async Task<int> SaveAsync(int p_id_prestamo, string p_empleado, string p_nombre)
    {
        try
        {

            var newLoanFile = new LoanFileEntity
            {
                // id_archivos = 0,
                id_prestamo = p_id_prestamo,
                empleado = p_empleado,
                ruta_completa = "",
                nombre_archivo = p_nombre,
                activo = true,
                fecha_creacion = DateTime.Now
            };
            _context.LoanFileEntity.Add(newLoanFile);

            return await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            return 0;
        }
    }
    
}
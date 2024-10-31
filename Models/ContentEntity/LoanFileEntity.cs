using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndRecursosHumanosFilasur.Models.ContentResponse;

public class LoanFileEntity
{
    public int? id_archivos { get; set; }
    public int? id_prestamo { get; set; }
    public string? empleado { get; set; }
    public string? ruta_completa { get; set; }
    public string? nombre_archivo { get; set; }
    public bool? activo { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? fecha_creacion {get; set;}
}
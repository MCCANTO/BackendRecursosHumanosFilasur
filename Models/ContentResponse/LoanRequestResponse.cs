using System.ComponentModel.DataAnnotations.Schema;

namespace BackendRecursosHumanosInkaCrops.Models.ContentResponse;

public class LoanRequestResponse
{
    public int id_prestamo {get; set;}
    public string? empleado { get; set;}
    public string nombre_completo {get; set;}
    public string dni {get; set;}
    public string area {get; set;}  
    public string puesto {get; set;}
    public string? empleado_obrero {get; set;}
    public decimal monto {get; set;}
    public string motivo {get; set;}
    public int nro_meses_descuento {get; set;}
    public bool descuento_julio {get; set;}
    public decimal valor_descuento_julio {get; set;}
    public bool descuento_diciembre {get; set;}
    public decimal valor_descuento_diciembre {get; set;}
    [Column(TypeName = "datetime")]
    public DateTime fecha_solicitud {get; set;}
    public bool confirmacion_trabajador {get; set;}
    public bool confirmacion_bs {get; set;}
    public bool confirmacion_gerente_rh {get; set;}

    [Column(TypeName = "datetime")]
    public DateTime fecha_creacion {get; set;}
    public string? usuario_creacion {get; set;}
    [Column(TypeName = "datetime")]
    public DateTime? fecha_actualizacion {get; set;}
    public string? usuario_actualizacion {get; set;}

    public List<LoanAmountsResponse>? Descuentos { get; set;}
}


public class LoanAmountsResponse
{
    public int id_prestamo_montos { get; set; }
    public int numero { get; set; }
    public decimal valor_descuento { get; set; }
    public int id_prestamo { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime fecha_creacion { get; set; }
    public string usuario_creacion { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime fecha_actualizacion { get; set; }
    public string usuario_actualizacion { get; set; }
}
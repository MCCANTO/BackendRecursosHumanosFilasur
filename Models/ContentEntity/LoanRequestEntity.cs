using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace BackEndRecursosHumanosFilasur.Models.ContentResponse;

public class LoanRequestEntity
{
    public int id_prestamo {get; set;}
    public string? empleado { get; set; }
    public string? nombre_completo {get; set;}
    public string? dni {get; set;}
    public string? area {get; set;}  
    public string? puesto {get; set;}
    public string? correo {get; set;}
    public string? empleado_obrero {get; set;}
    public decimal monto {get; set;}
    public int id_motivo {get; set;}
    public int? nro_meses_descuento {get; set;}
    public int? nro_semanas_descuento { get; set;}
    public bool? descuento_julio {get; set;}
    public decimal? valor_descuento_julio {get; set;}
    public bool? descuento_diciembre {get; set;}
    public decimal valor_descuento_diciembre {get; set;}
    [Column(TypeName = "datetime")]
    public DateTime fecha_solicitud {get; set;}
    public bool? confirmacion_trabajador {get; set;}
    public bool? confirmacion_bs {get; set;}
    public bool? confirmacion_gerente_rh {get; set;}

    [Column(TypeName = "datetime")]
    public DateTime? fecha_creacion {get; set;}
    public string? usuario_creacion {get; set;}
    [Column(TypeName = "datetime")]
    public DateTime? fecha_actualizacion {get; set;}
    public string? usuario_actualizacion {get; set;}
    public string? motivo_rechazo_bs { get; set;}
    public string? motivo_rechazo_ger { get; set;}
    public bool? estado { get; set;}

}


/* DETALLE- SOLICITUD PRESTAMO*/
public class LoanAmountsEntity
{
    public int id_prestamo_montos {get;set;}
    public int numero {get;set;}
    public string? anio {get;set;}
    public string? tipo {get;set;}
    public decimal valor_descuento {get;set;}
    public int id_prestamo {get;set;}
    [Column(TypeName = "datetime")]
    public DateTime fecha_creacion {get;set;}
    public string? usuario_creacion {get;set;}
    [Column(TypeName = "datetime")]
    public DateTime fecha_actualizacion {get;set;}
    public string? usuario_actualizacion {get;set;}
}

public class LoanRequestFilentity
{
    public int id_archivos { get; set; }
    public int id_prestamo { get; set; }
    public string? empleado { get; set; }
    public string? ruta_completa { get; set; }
    public string? nombre_archivo { get; set; }
    public bool activo { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime fecha_creacion { get; set; }
}

/* DETALLE- SOLICITUD PRESTAMO*/

public class LoanReasonEntity
{
    public int id_motivo { get; set; }
    public string? tipo_motivo { get; set; }
    public string? descripcion { get; set; }
    public bool estado_motivo { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime fecha_creacion { get; set; }
    public string? usuario_creacion { get; set; }
}


public class LoanReasonRejectionEntity
{
    public int id { get; set; }
    public int? id_solicitud { get; set; }
    public string? tipo_solicitud { get; set; }
    public string? motivo_rechazo { get; set; }
    public string? usuario { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime fecha_rechazo { get; set; }
}
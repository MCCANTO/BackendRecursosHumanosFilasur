using BackEndRecursosHumanosFilasur.Models.ContentBody;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndRecursosHumanosFilasur.Models.ContentResponse
{
    public class LoanEmployeeResponse
    {
        public string? nombre {  get; set; }
        public string? dni { get; set; }
        public string? area { get; set; }
        public string? puesto { get; set; }
        public string? correo { get; set; }
        public int? prestamo_activo { get; set; }
        public int? adelanto_activo { get; set; }
    }

    public class RequestValidationState
    {
        public bool? validacion { get; set; }
    }

    public class SalarayAdvanceEmployeeAccount
    {
        public string? nombre { get; set; }
        public string? dni { get; set; }
        public string? nro_cuenta_or_cci { get; set; }
        public string? correo { get; set; }
    }

    public class LoanLimitResponse
    {
        public decimal monto_limite_prestamo { get; set; }
        public decimal salario { get; set; }
    }

    

    public class LoanCreatedRequestResponse
    {
        public int id_prestamo { get; set; }
        public string? empleado { get; set; }
        public string? nombre_completo { get; set; }
        public string? dni { get; set; }
        public string? area { get; set; }
        public string? puesto { get; set; }
        public string? correo { get; set; }
        public string? empleado_obrero { get; set; }
        public decimal? monto { get; set; }
        public string? descripcion { get; set; }//id_motivo
        public int? nro_meses_descuento { get; set; }
        public bool? descuento_julio { get; set; }
        public decimal? valor_descuento_julio { get; set; }
        public bool? descuento_diciembre { get; set; }
        public decimal? valor_descuento_diciembre { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha_solicitud { get; set; }
        public bool? confirmacion_trabajador { get; set; }
        public bool? confirmacion_bs { get; set; }
        public bool? confirmacion_gerente_rh { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? fecha_creacion { get; set; }
        public string? usuario_creacion { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? fecha_actualizacion { get; set; }
        public string? usuario_actualizacion { get; set; }
        public bool? estado { get; set; }
    }
}

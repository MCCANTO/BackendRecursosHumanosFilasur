﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndRecursosHumanosFilasur.Models.ContentBody
{
    public class AdvanceSalaryBody
    {
        public int id_adelanto_sueldo { get; set; }
        public string? nombre_completo { get; set; }
        public string? dni { get; set; }
        public decimal? monto { get; set; }
        public int id_motivo { get; set; }
        public string? correo { get; set; }
        public bool? salario_sueldo { get; set; }
        public string? nro_cuenta_or_cci { get; set; }
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
        public string? motivo_rechazo_bs { get; set; }
        public string? motivo_rechazo_ger { get; set; }
        public string? empleado { get; set; }
        public bool? estado {get; set; }  
    }
}

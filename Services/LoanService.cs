using BackEndRecursosHumanosFilasur.Data;
using BackEndRecursosHumanosFilasur.Helpers;
using BackEndRecursosHumanosFilasur.Models.ContentBody;
using BackEndRecursosHumanosFilasur.Models.ContentResponse;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.TermStore;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BackEndRecursosHumanosFilasur.Services;

public class LoanService : _BaseService
{
    public LoanService(BaseContext context) : base(context) { }
    
    public async Task<object> GetAllLoanRequest(DateTime fechaInicio, DateTime fechaFin, string buscar, string usercode)
    {
        try
        {
            var result = await _context.LoanCreatedRequestResponse.FromSqlInterpolated($"exec filasur.xtus_rh_solicitudes_prestamo_aprobar {usercode}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    
    public async Task<object> GetAllEmployee(string empleado)
    {
        try
        {
            var result = await _context.LoanEmployeeResponse.FromSqlInterpolated($"exec filasur.xtus_rh_obtener_empleado {empleado}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }



    public async Task<object> RegistrarPrestamo(LoanBody loan)
    {
            try
            {
                string empleado_valor = loan.puesto.Contains("OPERARIO", StringComparison.OrdinalIgnoreCase) ? "O" : "E";
                string valor_tipo;

                var newLoan = new LoanRequestEntity()
                {
                    nombre_completo = loan.nombre_completo,
                    dni = loan.dni,
                    area = loan.area,
                    puesto = loan.puesto,
                    monto = loan.monto,
                    id_motivo = loan.id_motivo,
                    empleado = loan.empleado,
                    empleado_obrero = empleado_valor,
                    correo = loan.correo,
                    fecha_solicitud = DateTime.Now,
                    confirmacion_trabajador = false,
                    confirmacion_bs = false,
                    confirmacion_gerente_rh = false,
                    nro_meses_descuento = loan.nro_meses_descuento,
                    nro_semanas_descuento = loan.nro_semanas_descuento,
                    descuento_julio = loan.descuento_julio,
                    valor_descuento_julio = loan.valor_descuento_julio,
                    descuento_diciembre = loan.descuento_julio,
                    valor_descuento_diciembre = loan.valor_descuento_diciembre,
                    fecha_actualizacion = DateTime.Now,
                    fecha_creacion = DateTime.Now,
                    estado = true
                };

              
                _context.LoanRequestEntity.Add(newLoan);
                await _context.SaveChangesAsync();

                if (loan.descuentos != null && loan.descuentos.Any())
                {
                    foreach (var descuento in loan.descuentos)
                    {
                    if (empleado_valor == "O")
                    {
                        valor_tipo = "Semana";
                    }
                    else { valor_tipo = "Mes"; }

                        var detalleEntity = new LoanAmountsEntity()
                        {
                            id_prestamo = newLoan.id_prestamo,
                            numero = descuento.numero,
                            anio = descuento.anio,
                            tipo = valor_tipo,
                            valor_descuento = descuento.valor_descuento,
                            fecha_creacion = DateTime.Now,
                            fecha_actualizacion = DateTime.Now,
                            usuario_creacion = loan.dni
                        };
                        _context.LoanAmountsEntity.Add(detalleEntity);
                    }
                    await _context.SaveChangesAsync();
                }

                /**********ENVIAR CORREO*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.Mensajes.AsuntoSolicitudPrestamo;
                var msgCorreoHtml = AppConfig.Mensajes.MensajeJefatura
                                                        .Replace("{nombre_completo}", loan.nombre_completo)
                                                        .Replace("{area}", loan.area)
                                                        .Replace("{website}", AppConfig.Configuracion.Website);

                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);

                /*Enviar correo al usuario sobre indicándole el registro de su solicitud*/
                var asuntoSolicitudRegisrada = AppConfig.Mensajes.AsuntoSolicitudPrestamoRegistrada;
                var msgCorreoUsuarioHtml = AppConfig.Mensajes.MensajeUsuarioSolicitudRegistrada.Replace("{nombre_completo}", loan.nombre_completo).Replace("{area}", loan.area);


                //bool status1 = await mail.EnviarCorreoAsync(/*newLoan.correo!*/AppConfig.Configuracion.DestinoEmailJefatura, asuntoSolicitudRegisrada, msgCorreoUsuarioHtml, esHtlm: true);
                bool status1 = await mail.EnviarCorreoAsync(newLoan.correo!, asuntoSolicitudRegisrada, msgCorreoUsuarioHtml, esHtlm: true);

                return new { message = "solicitud registrada", prestamo= newLoan.id_prestamo, errorCode = 1 };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return new { message = "Error en la fecha: " + ex.Message, errorCode = 0 };
            }
            catch (SqlTypeException ex)
            {
                return new { message = "Error en la fecha SQL: " + ex.Message, errorCode = 0 };
            }
            catch (DbUpdateException ex)
            {
                return new { message = "Error al actualizar la base de datos", errorCode = 0, details = ex.Message, innerException = ex.InnerException?.Message };
            }
            catch (Exception ex)
            {
                return new { message = "Error al registrar la solicitud", errorCode = 0, details = ex.Message, innerException = ex.InnerException?.Message };
            }
    }

    // public async Task<object> SaveAsyncFile()
    // {
    //     try
    //     {
    //         var rutaCompleta = AppConfig.Configuracion.CarpetaArchivos;
    //         var activo = true;
    //         var fechaCreacion = DateTime.Now;
    //
    //         Console.WriteLine($"Ruta Completa: {rutaCompleta}, Activo: {activo}, Fecha Creacion: {fechaCreacion}");
    //
    //         var file = new LoanRequestFilentity
    //         {
    //             ruta_completa = rutaCompleta,
    //             activo = activo,
    //             fecha_creacion = fechaCreacion
    //         };
    //
    //         _context.LoanRequestFileEntity.Add(file);
    //         await _context.SaveChangesAsync();
    //
    //         return new { message = "solicitud registrada", errorCode = 1 };
    //     }
    //     catch (System.Exception ex)
    //     {
    //         return new { message = "Error al registrar la solicitud", errorCode = 0, details = ex.Message, innerException = ex.InnerException?.Message };
    //     }
    // }

    public async Task<object> GetReasonLoanAsync()
    {
        try
        {
            int currentTime = DateTime.Now.Month;

            var query = from motivo_solicitud in _context.LoanReasonEntity
                        where motivo_solicitud.tipo_motivo == "P" && motivo_solicitud.estado_motivo == true
                        select motivo_solicitud;

            // Crear la proyección según el mes actual
            var result = (currentTime >= 1 && currentTime <= 3)
                ? await query.Select(motivo_solicitud => new
                {
                    motivo_solicitud.id_motivo,
                    motivo_solicitud.descripcion
                }).ToListAsync()

                : await query.Where(motivo_solicitud => motivo_solicitud.id_motivo != 4)
                             .Select(motivo_solicitud => new
                {
                    motivo_solicitud.id_motivo,
                    motivo_solicitud.descripcion
                }).ToListAsync();


            //var result = await query.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetLoanApplicationCreated(string usercode)
    {
        try
        {
            var result = await _context.LoanCreatedRequestResponse.FromSqlInterpolated($"exec filasur.xtus_rh_solicitudes_prestamo {usercode}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> UpdateLoanEmployeeConfirmation(LoanBody loan)
    {
        try
        {
            var currentLoan = await _context.LoanRequestEntity.FindAsync(loan.id_prestamo);

            if (currentLoan != null)
            {
                currentLoan.confirmacion_trabajador = loan.confirmacion_trabajador;

                _context.LoanRequestEntity.Update(currentLoan);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO CONFORMIDAD*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.Mensajes.AsuntoSolicitudUsuarioConformidad;
                var msgCorreoHtml = AppConfig.Mensajes.MensajeUsuarioConformidad
                                                        .Replace("{nombre_completo}", currentLoan.nombre_completo)
                                                        .Replace("{area}",currentLoan.area)
                                                        .Replace("{id_prestamo}",currentLoan.id_prestamo.ToString())
                                                        .Replace("{website}",AppConfig.Configuracion.Website);

                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);

                /*Enviar correo a RRHH Serviam */
                var asuntoSolicitudPendiente = AppConfig.MensajeRRHHServiam.AsuntoSolicitudPrestamoPendiente;
                var msgCorreoServiamHtml = AppConfig.MensajeRRHHServiam.MensajeSolicitudPrestamoPendiente;

               
                bool status2 = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailRRHH, asuntoSolicitudPendiente, msgCorreoServiamHtml, esHtlm: true);

                return new { message = "Estado de conformidad de empleado actualizada exitosamente", errorCode = 1 };
            }

            return new { message = "No se actualizar el estado", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }


    public async Task<object> UpdateLoanSupervisorApproval(LoanBody loan)
    {
        try
        {
            var currentLoan = await _context.LoanRequestEntity.FindAsync(loan.id_prestamo);

            if (currentLoan != null)
            {
                currentLoan.confirmacion_bs = loan.confirmacion_bs;

                _context.LoanRequestEntity.Update(currentLoan);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO JEFATURA*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.Mensajes.AsuntoSolicitudPrestamo;
                var msgCorreoHtml = AppConfig.Mensajes.MensajeGerencia
                                                        .Replace("{website}", AppConfig.Configuracion.Website);

                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailGerencia, asunto, msgCorreoHtml, esHtlm: true);


                return new { message = "Estado de aprobación jefatura actualizada exitosamente", errorCode = 1 };
           }
             
            return new { message = "No se actualizar el estado", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }

    public async Task<object> UpdateLoanManagerApproval(LoanBody loan)
    {
        try
        {
            List<string> emails = new List<string>();
            

            var currentLoan = await _context.LoanRequestEntity.FindAsync(loan.id_prestamo);

            if (currentLoan != null)
            {
                currentLoan.confirmacion_gerente_rh = loan.confirmacion_gerente_rh;

                _context.LoanRequestEntity.Update(currentLoan);
                await _context.SaveChangesAsync();

                /*Enviar correo comunicando la aprobación de sp*/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.Mensajes.AsuntoSolicitudPrestamoAprobado;
                var msgCorreoHtml = AppConfig.Mensajes.MensajeAprobacionGerencia
                                                            .Replace("{id_prestamo}", loan.id_prestamo.ToString())
                                                            .Replace("{website}", AppConfig.Configuracion.Website);

                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);

                /*Enviar correo al usuario sobre estado de su sp*/
                var asuntoEstadoSolicitud = AppConfig.Mensajes.AsuntoSolicitudPrestamoAprobado;
                var msgCorreoUsuarioHtml = AppConfig.Mensajes.MensajeUsuarioSolicitud
                                                        .Replace("{nombre_completo}", currentLoan.nombre_completo)
                                                        .Replace("{area}", currentLoan.area)
                                                        .Replace("{website}", AppConfig.Configuracion.Website);


                //bool status1 = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asuntoEstadoSolicitud, msgCorreoUsuarioHtml, esHtlm: true);
                bool status1 = await mail.EnviarCorreoAsync(currentLoan.correo!, asuntoEstadoSolicitud, msgCorreoUsuarioHtml, esHtlm: true);

                return new { message = "Estado de aprobación gerencia actualizada exitosamente", errorCode = 1 };
            }

            return new { message = "No se actualizar el estado", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }

    public async Task<object> GetApprovalRequestAsync()
    {
        try
        {
            var query = from prestamo in _context.LoanRequestEntity
                        where prestamo.confirmacion_bs == true
                        select new
                        {
                            prestamo.id_prestamo,
                            prestamo.nombre_completo,
                            prestamo.area,
                            prestamo.puesto,
                            prestamo.monto,
                            prestamo.nro_meses_descuento,
                            prestamo.descuento_julio,
                            prestamo.valor_descuento_julio,
                            prestamo.descuento_diciembre,
                            prestamo.valor_descuento_diciembre,
                            prestamo.fecha_creacion,
                            prestamo.confirmacion_bs                      
                        };

            var result = await query.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetLoanDetails(int id_prestamo)
    {
        try
        {
            var result = await _context.LoanAmountsEntities.Where(ld => ld.id_prestamo == id_prestamo).ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }


    public async Task<object> GetLoanLimitAmount(string empleado)
    {
        try
        {
            var result = await _context.LoanLimitResponse.FromSqlInterpolated($"exec filasur.xtus_rh_obtener_monto_limite_prestamo {empleado}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }


    public async Task<object> LoanRequestDeniedSupervisor(LoanBody loan)
    {
        try
        {
            var currentLoan = await _context.LoanRequestEntity.FindAsync(loan.id_prestamo);

            if (currentLoan != null)
            {
                currentLoan.estado = loan.estado;
                currentLoan.motivo_rechazo_bs = loan.motivo_rechazo_bs;
                _context.LoanRequestEntity.Update(currentLoan);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO INDICANDO RECHAZO DE SP*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.Mensajes.AsuntoSolicitudRechazo;
                var msgCorreoHtml = AppConfig.Mensajes.MensajeUsuarioRechazo
                    .Replace("{nombre_completo}", currentLoan.nombre_completo)
                    .Replace("{motivo_rechazo_bs}", currentLoan.motivo_rechazo_bs)
                    .Replace("{area}", currentLoan.area)
                    .Replace("{id_prestamo}", currentLoan.id_prestamo.ToString());
                //cambiar al correo usuario
                bool status = await mail.EnviarCorreoAsync(currentLoan.correo!, asunto, msgCorreoHtml, esHtlm: true);

               //bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);


                return new { message = "Se rechazó de solicitud de prestamo exitosamente", errorCode = 1 };
            }

            return new { message = "Error al rechazar la solicitud de prestamo por el area de bienestar social", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }

    public async Task<object> LoanRequestDeniedManager(LoanBody loan)
    {
        
        try
        {
            var currentLoan = await _context.LoanRequestEntity.FindAsync(loan.id_prestamo);

            if (currentLoan != null)
            {
                currentLoan.confirmacion_bs = false;
                currentLoan.motivo_rechazo_ger = loan.motivo_rechazo_ger;

                _context.LoanRequestEntity.Update(currentLoan);
                await _context.SaveChangesAsync();

                var motivoActualizado = await _context.LoanRequestEntity.FindAsync(loan.motivo_rechazo_bs);

                /**********ENVIAR CORREO INDICANDO RECHAZO GERENCIA DE SP*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.Mensajes.AsuntoSolicitudRechazo;
                var msgCorreoHtml = AppConfig.Mensajes.MensajeUsuarioRechazoGerencia
                    .Replace("{id_prestamo}", currentLoan.id_prestamo.ToString())
                    .Replace("{website}", AppConfig.Configuracion.Website);
                //cambiar al correo usuario
                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);


                return new { message = "El área de gerencia rechazó de solicitud de prestamo exitosamente", errorCode = 1 };
            }

            return new { message = "Error al rechazar la solicitud de prestamo por el area de gerencia", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }

    public async Task<object> GetRequestState(string empleado)
    {
        try
        {
            var result = await _context.RequestValidationState.FromSqlInterpolated($"exec filasur.xtus_rh_validar_solicitud_adelanto {empleado}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetRequestAllowedState(string empleado)
    {
        try
        {
            var result = await _context.RequestValidationState.FromSqlInterpolated($"exec filasur.xtus_rh_validacion_creacion_solicitud {empleado}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    
    public async Task<object> GetReasonLoanAsyncAll()
    {
        try
        {
            // Asumiré que _context es tu instancia de DbContext
            var result = await _context.LoanRequestEntity.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }
}
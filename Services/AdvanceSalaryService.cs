using BackEndRecursosHumanosFilasur.Data;
using BackEndRecursosHumanosFilasur.Helpers;
using BackEndRecursosHumanosFilasur.Models.ContentBody;
using BackEndRecursosHumanosFilasur.Models.ContentResponse;
using Microsoft.EntityFrameworkCore;

namespace BackEndRecursosHumanosFilasur.Services;

public class AdvanceSalaryService : _BaseService
{
    public AdvanceSalaryService(BaseContext context) : base(context) { }

    public async Task<object> GetAllAdvancedSalaryEv(string usercode)
    {
        try
        {
            var result = await _context.AdvancedSalaryRequestEntity.FromSqlInterpolated($"exec filasur.xtus_rh_solicitudes_adelanto_sueldo_aprobar {usercode}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> GetAllAdvancedSalaryRequest(string usercode)
    {
        try
        {
            var result = await _context.AdvancedSalaryRequestEntity.FromSqlInterpolated($"exec filasur.xtus_rh_solicitudes_adelanto_sueldo {usercode}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }
    public async Task<object> SaveAdvanceSalary(AdvanceSalaryBody advanceSalary)
    {
        try
        {
            var newAdvanceSalary = new AdvancedSalaryRequestEntity()
            {
                nombre_completo = advanceSalary.nombre_completo,
                dni = advanceSalary.dni,
                monto = advanceSalary.monto,
                nro_cuenta_or_cci = advanceSalary.nro_cuenta_or_cci,
                id_motivo = advanceSalary.id_motivo,
                correo = advanceSalary.correo,
                confirmacion_trabajador = false,
                confirmacion_bs = false,
                confirmacion_gerente_rh = false,
                fecha_creacion = DateTime.Now,
                fecha_solicitud = DateTime.Now,
                empleado = advanceSalary.empleado,
                estado = true,
            };
            _context.AdvancedSalaryRequestEntity.Add(newAdvanceSalary);
            await _context.SaveChangesAsync();

            /**********ENVIAR CORREO JEFATURA*********************/
            MailManagerHelper mail = new MailManagerHelper();
            var asunto = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelanto;
            var msgCorreoHtml = AppConfig.MensajesAdelatoSueldo.MensajeJefatura
                                                        .Replace("{nombre_completo}", advanceSalary.nombre_completo)
                                                        .Replace("{website}", AppConfig.Configuracion.Website);

            bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);

            /**********ENVIAR CORREO USUARIO*********************/
            var asuntoSolicitudRegistrada = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelantoResgistrada;
            var msgCorreoUserHtml = AppConfig.MensajesAdelatoSueldo.MensajeUsuarioSolicitudRegistrada
                                                            .Replace("{nombre_completo}", advanceSalary.nombre_completo)
                                                            .Replace("{website}", AppConfig.Configuracion.Website);
            //produccion-enviar correo a user
            //bool status1 = await mail.EnviarCorreoAsync(newAdvanceSalary.correo!, asuntoSolicitudRegistrada, msgCorreoUserHtml, esHtlm: true);
            
             bool status1 = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asuntoSolicitudRegistrada, msgCorreoUserHtml, esHtlm: true);


            return new { message = "Solicitud de adelanto de sueldo registrada correctamente", adelanto = newAdvanceSalary.id_adelanto_sueldo, errorCode = 1 };
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetAdvanceReasonAsync()
    {
        try
        {
            var query = from motivo in _context.LoanReasonEntity
                            where motivo.tipo_motivo == "A" && motivo.estado_motivo == true
                        select new
                        {
                            motivo.id_motivo,
                            motivo.descripcion
                        };

            var result = await query.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetSalaryAdvanceAccountEmployee(string empleado)
    {
        try
        {
            var result = await _context.SalarayAdvanceEmployeeAccount.FromSqlInterpolated($"exec filasur.xtus_rh_obtener_empleado_adelanto_sueldo {empleado}").ToListAsync();
            if (result == null) return new object[] { };
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetAdvanceSalarySupervisor()
    {
        try
        {
            var query = from adelanto in _context.AdvancedSalaryRequestEntity
                        join motivo in _context.LoanReasonEntity on adelanto.id_motivo equals motivo.id_motivo
                        //where adelanto.confirmacion_bs == true
                        select new
                        {
                            adelanto.id_adelanto_sueldo,
                            adelanto.nombre_completo,
                            adelanto.monto,
                            motivo.descripcion,
                            adelanto.fecha_solicitud,
                            adelanto.confirmacion_trabajador,
                            adelanto.confirmacion_bs,
                            adelanto.confirmacion_gerente_rh
                        };

            var result = await query.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> GetAdvanceSalaryManager()
    {
        try
        {
            var query = from adelanto in _context.AdvancedSalaryRequestEntity
                        join motivo in _context.LoanReasonEntity on adelanto.id_motivo equals motivo.id_motivo
                            where adelanto.confirmacion_bs == true
                        select new
                        {
                            adelanto.id_adelanto_sueldo,
                            adelanto.nombre_completo,
                            adelanto.monto,
                            motivo.descripcion,
                            adelanto.fecha_solicitud,
                            adelanto.confirmacion_bs,
                            adelanto.confirmacion_gerente_rh
                        };

            var result = await query.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> UpdateAdvanceSalarySupervisorApproval(AdvanceSalaryBody advance)
    {
        try
        {
            var currentAdvance = await _context.AdvancedSalaryRequestEntity.FindAsync(advance.id_adelanto_sueldo);

            if (currentAdvance != null)
            {
                currentAdvance.confirmacion_bs = advance.confirmacion_bs;
                currentAdvance.fecha_actualizacion = DateTime.Now;

                _context.AdvancedSalaryRequestEntity.Update(currentAdvance);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO JEFATURA*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelanto;
                var msgCorreoHtml = AppConfig.MensajesAdelatoSueldo.MensajeGerencia
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

    public async Task<object> UpdateAdvanceSalaryManagerApproval(AdvanceSalaryBody advance)
    {
        try
        {
            var currentAdvance = await _context.AdvancedSalaryRequestEntity.FindAsync(advance.id_adelanto_sueldo);

            if (currentAdvance != null)
            {
                currentAdvance.confirmacion_gerente_rh = advance.confirmacion_gerente_rh;
                currentAdvance.fecha_actualizacion = advance.fecha_actualizacion;
                   
                _context.AdvancedSalaryRequestEntity.Update(currentAdvance);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO JEFATURA-CONFIRMACIÓN*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelantoAprobado;
                var msgCorreoHtml = AppConfig.MensajesAdelatoSueldo.MensajeAprobacionGerencia
                                                                    .Replace("{id_prestamo}", currentAdvance.id_adelanto_sueldo.ToString())
                                                                    .Replace("{website}", AppConfig.Configuracion.Website);

                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);

                /**********ENVIAR CORREO USUARIO*********************/
                var asuntoSolicitudRegistrada = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelantoAprobado;
                var msgCorreoUserHtml = AppConfig.MensajesAdelatoSueldo.MensajeUsuarioSolicitud
                                                            .Replace("{nombre_completo}", currentAdvance.nombre_completo)
                                                            .Replace("{website}", AppConfig.Configuracion.Website);

                //bool status1 = await mail.EnviarCorreoAsync(currentAdvance.correo!, asuntoSolicitudRegistrada, msgCorreoUserHtml, esHtlm: true);
                
                bool status1 = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asuntoSolicitudRegistrada, msgCorreoUserHtml, esHtlm: true);


                return new { message = "Estado de aprobación gerencia actualizada exitosamente", errorCode = 1 };
            }

            return new { message = "No se actualizar el estado", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }

    public async Task<object> UpdateAdvanceSalaryEmployeeApproval(AdvanceSalaryBody advance)
    {
        try
        {
            var currentAdvance = await _context.AdvancedSalaryRequestEntity.FindAsync(advance.id_adelanto_sueldo);

            if (currentAdvance != null)
            {
                currentAdvance.confirmacion_trabajador = advance.confirmacion_trabajador;
                currentAdvance.fecha_actualizacion = advance.fecha_actualizacion;

                _context.AdvancedSalaryRequestEntity.Update(currentAdvance);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO EMPLEADO-CONFIRMACIÓN*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudUsuarioConformidad;
                var msgCorreoHtml = AppConfig.MensajesAdelatoSueldo.MensajeUsuarioConformidad
                                                        .Replace("{nombre_completo}", currentAdvance.nombre_completo)
                                                        .Replace("{id_adelanto_sueldo}", currentAdvance.id_adelanto_sueldo.ToString())
                                                        .Replace("{website}", AppConfig.Configuracion.Website);

                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);

                /**********ENVIAR CORREO RRHH Serviam*********************/
                var asuntoAdelantoPendiente = AppConfig.MensajeRRHHServiam.AsuntoSolicitudAdelantoPendiente;
                var msgServiamUserHtml = AppConfig.MensajeRRHHServiam.MensajeSolicitudAdelantoPendiente;

                //var nombreArchivoExcel = "RRHH-ADELANTOS-PRESTAMOS-INKACROPS.xlsx";

                bool status2 = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailRRHH, asuntoAdelantoPendiente, msgServiamUserHtml, esHtlm: true);


                return new { message = "Estado de conformidad de empleado actualizada exitosamente", errorCode = 1 };
            }

            return new { message = "No se actualizar el estado", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }

    public async Task<object> GetLimitAdvance(string empleado)
    {
        try
        {
            var result = await _context.LimitAdvanceResponse.FromSqlInterpolated($"exec filasur.xtus_rh_obtener_monto_maximo_adelanto {empleado}").ToListAsync();
           
            if (result == null) return new object[] { };
            
            return result;
        }
        catch (System.Exception)
        {
            return new object[] { };
        }
    }

    public async Task<object> AdvanceRequestDeniedSupervisor(AdvanceSalaryBody advance)
    {
        try
        {
            var currentAdvance = await _context.AdvancedSalaryRequestEntity.FindAsync(advance.id_adelanto_sueldo);

            if (currentAdvance != null)
            {
                currentAdvance.estado = advance.estado;
                currentAdvance.motivo_rechazo_bs = advance.motivo_rechazo_bs;
                _context.AdvancedSalaryRequestEntity.Update(currentAdvance);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO INDICANDO RECHAZO DE SP*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudUsuarioRechazo;
                var msgCorreoHtml = AppConfig.MensajesAdelatoSueldo.MensajeUsuarioRechazo
                    .Replace("{nombre_completo}", currentAdvance.nombre_completo)
                    .Replace("{id_adelanto_sueldo}", currentAdvance.id_adelanto_sueldo.ToString())
                    .Replace("{motivo_rechazo_bs}", currentAdvance.motivo_rechazo_bs);
                //cambiar al correo usuario
                //bool status = await mail.EnviarCorreoAsync(currentAdvance.correo!, asunto, msgCorreoHtml, esHtlm: true);
                
                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);


                return new { message = "Se rechazó de solicitud de adelanto de sueldo exitosamente", errorCode = 1 };
            }

            return new { message = "Error al rechazar la solicitud de adelanto de sueldo por bienestar social", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }

    public async Task<object> AdvanceRequestDeniedManager(AdvanceSalaryBody advance)
    {
        try
        {
            var currentAdvance = await _context.AdvancedSalaryRequestEntity.FindAsync(advance.id_adelanto_sueldo);

            if (currentAdvance != null)
            {
                currentAdvance.confirmacion_bs = false;
                currentAdvance.motivo_rechazo_ger = advance.motivo_rechazo_ger;
                _context.AdvancedSalaryRequestEntity.Update(currentAdvance);
                await _context.SaveChangesAsync();

                /**********ENVIAR CORREO INDICANDO RECHAZO DE SP*********************/
                MailManagerHelper mail = new MailManagerHelper();
                var asunto = AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudUsuarioRechazo;
                var msgCorreoHtml = AppConfig.MensajesAdelatoSueldo.MensajeUsuarioRechazoGerencia
                                                        .Replace("{id_adelanto_sueldo}", currentAdvance.id_adelanto_sueldo.ToString())
                                                        .Replace("{website}", AppConfig.Configuracion.Website);
                //cambiar al correo usuario
                //bool status = await mail.EnviarCorreoAsync(currentAdvance.correo!, asunto, msgCorreoHtml, esHtlm: true);
                
                bool status = await mail.EnviarCorreoAsync(AppConfig.Configuracion.DestinoEmailJefatura, asunto, msgCorreoHtml, esHtlm: true);


                return new { message = "El área de gerencia rechazó de solicitud de adelanto exitosamente", errorCode = 1 };
            }

            return new { message = "Error al rechazar la solicitud de adelanto de sueldo por Gerencia", errorCode = 0 };
        }
        catch (System.Exception ex)
        {
            return new { message = ex.Message, errorCode = 0 };
        }
    }


    public async Task<object> GetSalaryAdvanceStatusRequest(string empleado)
    {
        try
        {
            // Realiza la consulta para verificar si existe alguna solicitud con estado == true
            var exists = await _context.AdvancedSalaryRequestEntity
                                       .AnyAsync(adelanto => adelanto.estado == true && adelanto.empleado == empleado)
                                      || await _context.LoanRequestEntity.AnyAsync(prestamo => prestamo.estado == true &&
                                      prestamo.empleado == empleado);

            // Retorna true si existe alguna solicitud con estado == true, de lo contrario, retorna false
            return exists;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<object> GetSalaryAdvanceAsyncAll()
    {
        try
        {
            // Asumiré que _context es tu instancia de DbContext
            var result = await _context.AdvancedSalaryRequestEntity.ToListAsync();
            return result;
        }
        catch (Exception)
        {
            return new object[] { };
        }
    }
}
using System.Net;
using System.Net.Mail;


namespace BackEndRecursosHumanosFilasur.Helpers
{
    public class MailManagerHelper
    {
        private SmtpClient cliente;
        public MailManagerHelper()
        {
            cliente = new SmtpClient(AppConfig.Configuracion.ServidorMail, AppConfig.Configuracion.PuertoMail)
            {
                EnableSsl = AppConfig.Configuracion.EnableSSLMail,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(AppConfig.Configuracion.UserMail, AppConfig.Configuracion.PasswordMail)
            };
        }

        public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string mensaje, List<Attachment>? attachList = null, bool esHtlm = false)
        {
            bool status = false;
            MailMessage email = null;
            SmtpClient cliente = null;

            try
            {
                email = new MailMessage
                {
                    From = new MailAddress(AppConfig.Configuracion.UserMail),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = esHtlm
                };

                string[] destinatarios = destinatario.Replace(";", ",").Split(',');

                foreach (string toEmail in destinatarios)
                {
                    if (!UtilityHelper.IsValidEmail(toEmail)) continue;
                    email.To.Add(new MailAddress(toEmail));
                }

                if (email.To.Count == 0) return false;

                if (attachList != null && attachList.Count > 0)
                {
                    attachList.ForEach(att => email.Attachments.Add(att));
                }

                cliente = new SmtpClient(AppConfig.Configuracion.ServidorMail, AppConfig.Configuracion.PuertoMail)
                {
                    EnableSsl = AppConfig.Configuracion.EnableSSLMail,
                    Credentials = new NetworkCredential(AppConfig.Configuracion.UserMail, AppConfig.Configuracion.PasswordMail) // Asegúrate de configurar las credenciales correctas
                };

                await cliente.SendMailAsync(email);
                status = true;
            }
            catch (System.Exception)
            {
                status = false;
            }
            finally
            {
                email?.Dispose();
                cliente?.Dispose();
            }

            return status;
        }
    }
}

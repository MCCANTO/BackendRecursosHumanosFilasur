namespace BackEndRecursosHumanosFilasur
{
    public class AppConfig
    {
        public class Mensajes
        {
            public static string AsuntoSolicitudPrestamo { get; set; } = null!;
            public static string AsuntoSolicitudPrestamoRegistrada { get; set; } = null!;
            public static string AsuntoSolicitudPrestamoAprobado { get; set; } = null!;
            public static string AsuntoSolicitudUsuario { get; set; } = null!;
            public static string AsuntoSolicitudUsuarioConformidad { get; set; } = null!;
            public static string AsuntoSolicitudDesembolso { get; set; } = null!;
            public static string AsuntoSolicitudRechazo { get; set; } = null!;
            public static string MensajeJefatura { get; set; } = null!;
            public static string MensajeGerencia { get; set; } = null!;
            public static string MensajeAprobacionGerencia { get; set; } = null!;
            public static string MensajeUsuarioSolicitudRegistrada { get; set; } = null!;
            public static string MensajeUsuarioSolicitud { get; set; } = null!;
            public static string MensajeUsuarioConformidad { get; set; } = null!;
            public static string MensajeUsuarioDesembolso { get; set; } = null!;
            public static string MensajeUsuarioRechazo { get; set; } = null!;
            public static string MensajeUsuarioRechazoGerencia { get; set; } = null!;
        }

        public class MensajesAdelatoSueldo
        {
            public static string AsuntoSolicitudAdelanto { get; set; } = null!;
            public static string AsuntoSolicitudAdelantoResgistrada { get; set; } = null!;
            public static string AsuntoSolicitudAdelantoAprobado { get; set; } = null!;
            public static string AsuntoSolicitudUsuario { get; set; } = null!;
            public static string AsuntoSolicitudUsuarioConformidad { get; set; } = null!;
            public static string AsuntoSolicitudUsuarioRechazo { get; set; } = null!;
            public static string MensajeUsuarioSolicitudRegistrada { get; set; } = null!;
            public static string MensajeJefatura { get; set; } = null!;
            public static string MensajeGerencia { get; set; } = null!;
            public static string MensajeAprobacionGerencia { get; set; } = null!;
            public static string MensajeUsuarioSolicitud { get; set; } = null!;
            public static string MensajeUsuarioConformidad { get; set; } = null!;
            public static string MensajeUsuarioRechazo { get; set; } = null!;
            public static string MensajeUsuarioRechazoGerencia { get; set; } = null!;
        }

        public class MensajeRRHHServiam
        {
            public static string AsuntoSolicitudPrestamoPendiente { get; set; } = null!;
            public static string AsuntoSolicitudAdelantoPendiente { get; set; } = null!;
            public static string MensajeSolicitudPrestamoPendiente { get; set; } = null!;
            public static string MensajeSolicitudAdelantoPendiente { get; set; } = null!;
        }
        public class Configuracion
        {
            public static string Website { get; set; } = null!;
            public static string SolicitudUsuario { get; set; } = null!;
            public static string CarpetaArchivos { get; set; } = null!;
            public static string ServidorMail { get; set; } = null!;
            public static int PuertoMail { get; set; }
            public static bool EnableSSLMail { get; set; }
            public static string UserMail { get; set; } = null!;
            public static string PasswordMail { get; set; } = null!;
            public static string DestinoEmailJefatura { get; set; } = null!;
            public static string DestinoEmailGerencia { get; set; } = null!;
            public static string DestinoEmailRRHH { get; set; } = null!;
            public static string ClientId { get; set; } = null!;
            public static string TenantId { get; set; } = null!;
            public static string ClientSecret { get; set; } = null!;
        }
    }
}

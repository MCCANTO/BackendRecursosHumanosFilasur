using BackEndRecursosHumanosFilasur;
using BackEndRecursosHumanosFilasur.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*Información de AppConfig*/

/* Configuracion */
AppConfig.Configuracion.Website = builder.Configuration["appConfig:Configuracion:Website"];
AppConfig.Configuracion.SolicitudUsuario = builder.Configuration["appConfig:Configuracion:SolicitudUsuario"];
AppConfig.Configuracion.CarpetaArchivos = builder.Configuration["appConfig:Configuracion:CarpetaArchivos"];
AppConfig.Configuracion.DestinoEmailJefatura = builder.Configuration["AppConfig:Configuracion:DestinoEmailJefatura"];
AppConfig.Configuracion.DestinoEmailGerencia = builder.Configuration["AppConfig:Configuracion:DestinoEmailGerencia"];
AppConfig.Configuracion.DestinoEmailRRHH = builder.Configuration["AppConfig:Configuracion:DestinoEmailRRHH"];
AppConfig.Configuracion.EnableSSLMail = Convert.ToBoolean(builder.Configuration["appConfig:Configuracion:EnableSSLMail"]);
AppConfig.Configuracion.PasswordMail = builder.Configuration["appConfig:Configuracion:PasswordMail"];
AppConfig.Configuracion.PuertoMail = Convert.ToInt32(builder.Configuration["appConfig:Configuracion:PuertoMail"]);
AppConfig.Configuracion.ServidorMail = builder.Configuration["appConfig:Configuracion:ServidorMail"];
AppConfig.Configuracion.UserMail = builder.Configuration["appConfig:Configuracion:UserMail"];

AppConfig.Configuracion.ClientId = builder.Configuration["appConfig:Configuracion:ClientId"];
AppConfig.Configuracion.TenantId = builder.Configuration["appConfig:Configuracion:TenantId"];
AppConfig.Configuracion.ClientSecret = builder.Configuration["appConfig:Configuracion:ClientSecret"];



/* Mensajes */

AppConfig.Mensajes.AsuntoSolicitudPrestamo = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudPrestamo"];
AppConfig.Mensajes.AsuntoSolicitudPrestamoRegistrada = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudPrestamoRegistrada"];
AppConfig.Mensajes.AsuntoSolicitudPrestamoAprobado = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudPrestamoAprobado"];
AppConfig.Mensajes.AsuntoSolicitudUsuario = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudUsuario"];
AppConfig.Mensajes.AsuntoSolicitudRechazo = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudRechazo"];
AppConfig.Mensajes.MensajeUsuarioSolicitudRegistrada = builder.Configuration["appConfig:Mensajes:MensajeUsuarioSolicitudRegistrada"];
AppConfig.Mensajes.AsuntoSolicitudUsuarioConformidad = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudUsuarioConformidad"];
AppConfig.Mensajes.AsuntoSolicitudDesembolso = builder.Configuration["appConfig:Mensajes:AsuntoSolicitudDesembolso"];
AppConfig.Mensajes.MensajeJefatura = builder.Configuration["appConfig:Mensajes:MensajeJefatura"];
AppConfig.Mensajes.MensajeGerencia = builder.Configuration["appConfig:Mensajes:MensajeGerencia"];
AppConfig.Mensajes.MensajeAprobacionGerencia = builder.Configuration["appConfig:Mensajes:MensajeAprobacionGerencia"];
AppConfig.Mensajes.MensajeUsuarioSolicitud = builder.Configuration["appConfig:Mensajes:MensajeUsuarioSolicitud"];
AppConfig.Mensajes.MensajeUsuarioConformidad = builder.Configuration["appConfig:Mensajes:MensajeUsuarioConformidad"];
AppConfig.Mensajes.MensajeUsuarioDesembolso = builder.Configuration["appConfig:Mensajes:MensajeUsuarioDesembolso"];
AppConfig.Mensajes.MensajeUsuarioRechazo = builder.Configuration["appConfig:Mensajes:MensajeUsuarioRechazo"];
AppConfig.Mensajes.MensajeUsuarioRechazoGerencia = builder.Configuration["appConfig:Mensajes:MensajeUsuarioRechazoGerencia"];


/* Mensajes RRHH SERVIAM */
AppConfig.MensajeRRHHServiam.AsuntoSolicitudPrestamoPendiente = builder.Configuration["appConfig:MensajeRRHHServiam:AsuntoSolicitudPrestamoPendiente"];
AppConfig.MensajeRRHHServiam.AsuntoSolicitudAdelantoPendiente = builder.Configuration["appConfig:MensajeRRHHServiam:AsuntoSolicitudAdelantoPendiente"];
AppConfig.MensajeRRHHServiam.MensajeSolicitudPrestamoPendiente = builder.Configuration["appConfig:MensajeRRHHServiam:MensajeSolicitudPrestamoPendiente"];
AppConfig.MensajeRRHHServiam.MensajeSolicitudAdelantoPendiente = builder.Configuration["appConfig:MensajeRRHHServiam:MensajeSolicitudAdelantoPendiente"];


/* Mensajes Adelanto Sueldo */

AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelanto = builder.Configuration["appConfig:MensajesAdelatoSueldo:AsuntoSolicitudAdelanto"];
AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelantoResgistrada = builder.Configuration["appConfig:MensajesAdelatoSueldo:AsuntoSolicitudAdelantoResgistrada"];
AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudAdelantoAprobado = builder.Configuration["appConfig:MensajesAdelatoSueldo:AsuntoSolicitudAdelantoAprobado"];
AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudUsuario = builder.Configuration["appConfig:MensajesAdelatoSueldo:AsuntoSolicitudUsuario"];
AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudUsuarioConformidad = builder.Configuration["appConfig:MensajesAdelatoSueldo:AsuntoSolicitudUsuarioConformidad"];
AppConfig.MensajesAdelatoSueldo.AsuntoSolicitudUsuarioRechazo = builder.Configuration["appConfig:MensajesAdelatoSueldo:AsuntoSolicitudUsuarioRechazo"];
AppConfig.MensajesAdelatoSueldo.MensajeUsuarioSolicitudRegistrada = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeUsuarioSolicitudRegistrada"];
AppConfig.MensajesAdelatoSueldo.MensajeJefatura = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeJefatura"];
AppConfig.MensajesAdelatoSueldo.MensajeGerencia = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeGerencia"];
AppConfig.MensajesAdelatoSueldo.MensajeAprobacionGerencia = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeAprobacionGerencia"];
AppConfig.MensajesAdelatoSueldo.MensajeUsuarioSolicitud = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeUsuarioSolicitud"];
AppConfig.MensajesAdelatoSueldo.MensajeUsuarioConformidad = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeUsuarioConformidad"];
AppConfig.MensajesAdelatoSueldo.MensajeUsuarioRechazo = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeUsuarioRechazo"];
AppConfig.MensajesAdelatoSueldo.MensajeUsuarioRechazoGerencia = builder.Configuration["appConfig:MensajesAdelatoSueldo:MensajeUsuarioRechazoGerencia"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((x) => true);
    });
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 268435456;
});

builder.Services.AddDbContext<BaseContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("exactus")));
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});*/
app.UseRouting();

app.UseHttpsRedirection();
app.UseCors("NuevaPolitica");
// app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.Run();

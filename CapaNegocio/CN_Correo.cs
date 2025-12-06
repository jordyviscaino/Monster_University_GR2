using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Monster_University_GR2.CapaNegocio
{
    public class CN_Correo
    {
        public static bool Enviar(string correoDestino, string asunto, string mensaje, out string errorDetalle)
        {
            errorDetalle = "";
            try
            {
                // 1. Leer credenciales del appsettings.json
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                IConfiguration root = builder.Build();

                string correoEmisor = root["CorreoConfig:Email"];
                string passwordEmisor = root["CorreoConfig:Password"];
                string host = root["CorreoConfig:Host"];
                int port = int.Parse(root["CorreoConfig:Port"]);

                // 2. Configurar el Correo
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(correoEmisor, "Soporte Monster University");
                mail.To.Add(correoDestino);
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                // 3. Configurar el Servidor SMTP
                SmtpClient smtp = new SmtpClient(host);
                smtp.Port = port;

                // ORDEN CRÍTICO DE CONFIGURACIÓN
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false; // Desactivar credenciales por defecto
                smtp.Credentials = new NetworkCredential(correoEmisor, passwordEmisor); // Asignar las nuevas

                // 4. Enviar
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                errorDetalle = ex.Message;
                if (ex.InnerException != null) errorDetalle += " | " + ex.InnerException.Message;
                return false;
            }
        }
    }
}
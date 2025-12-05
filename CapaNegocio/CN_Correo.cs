using System;
using System.Net;
using System.Net.Mail;

namespace Monster_University_GR2.CapaNegocio
{
    public class CN_Correo
    {
        // Configuración de tu cuenta REMITENTE (la que envía)
        // OJO: Si usas autenticación de dos pasos, necesitas una "Contraseña de Aplicación"
        private static string _correoEmisor = "jordyviscainoa@hotmail.com";
        private static string _passwordEmisor = "psybeqevwevybwfz";

        public static bool Enviar(string correoDestino, string asunto, string mensaje)
        {
            try
            {
                // 1. Configurar el mensaje
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(_correoEmisor);
                mail.To.Add(correoDestino);
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true; // Para poder usar HTML en el correo (negritas, colores)

                // 2. Configurar el Servidor SMTP de Hotmail/Outlook
                SmtpClient smtp = new SmtpClient("smtp.office365.com");
                smtp.Port = 587; // Puerto estándar seguro
                smtp.EnableSsl = true; // Cifrado obligatorio
                smtp.Credentials = new NetworkCredential(_correoEmisor, _passwordEmisor);

                // 3. Enviar
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                // Aquí podrías guardar un log del error si falla
                Console.WriteLine("Error enviando correo: " + ex.Message);
                return false;
            }
        }
    }
}
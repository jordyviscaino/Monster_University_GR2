using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Monster_University_GR2.CapaNegocio
{
    public class ServicioCorreo
    {
        private readonly IConfiguration _configuration;

        public ServicioCorreo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreo(string destino, string asunto, string mensajeHtml)
        {
            var emailEmisor = _configuration["CorreoConfig:Email"];
            var password = _configuration["CorreoConfig:Password"];
            var host = _configuration["CorreoConfig:Host"];
            var port = int.Parse(_configuration["CorreoConfig:Port"]);

            var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(emailEmisor, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailEmisor, "Monster University Security"),
                Subject = asunto,
                Body = mensajeHtml,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(destino);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
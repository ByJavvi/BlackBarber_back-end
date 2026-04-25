using BlackBarberAPI.DTOs;
using BlackBarberAPI.Services.Contratos;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace BlackBarberAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailSettings:User").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Token };

            using var smtp = new SmtpClient();

            // Conexión al servidor (ejemplo con Gmail/Outlook)
            await smtp.ConnectAsync(
                _config.GetSection("EmailSettings:Host").Value,
                int.Parse(_config.GetSection("EmailSettings:Port").Value),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _config.GetSection("EmailSettings:User").Value,
                _config.GetSection("EmailSettings:Password").Value
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}

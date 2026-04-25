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
            var bodyHtml = "<!DOCTYPE html>\r\n<html lang=\"es\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Restablecer Contraseña</title>\r\n</head>\r\n<body style=\"margin: 0; padding: 0; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f4f4;\">\r\n    <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color: #f4f4f4; padding: 20px;\">\r\n        <tr>\r\n            <td align=\"center\">\r\n                <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);\">\r\n                    <tr>\r\n                        <td align=\"center\" style=\"padding: 40px 0; background-color: #2c3e50;\">\r\n                            <h1 style=\"color: #ffffff; margin: 0; font-size: 24px; letter-spacing: 1px;\">Seguridad de la Cuenta</h1>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"padding: 40px; color: #333333; line-height: 1.6;\">\r\n                            <h2 style=\"margin-top: 0; font-size: 20px;\">¿Olvidaste tu contraseña?</h2>\r\n                            <p>Recibimos una solicitud para restablecer la contraseña de tu cuenta. No te preocupes, puedes volver a ingresar haciendo clic en el botón de abajo:</p>\r\n                            \r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-top: 30px; margin-bottom: 30px;\">\r\n                                <tr>\r\n                                    <td align=\"center\">\r\n                                        <a href=\"{{URL_DE_RESTABLECIMIENTO}}\" target=\"_blank\" style=\"background-color: #3498db; color: #ffffff; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;\">\r\n                                            Restablecer Contraseña\r\n                                        </a>\r\n                                    </td>\r\n                                </tr>\r\n                            </table>\r\n                            \r\n                            <p style=\"font-size: 14px; color: #777777;\">Si no solicitaste este cambio, puedes ignorar este correo de forma segura. El enlace expirará pronto.</p>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td align=\"center\" style=\"padding: 20px; background-color: #f9f9f9; color: #999999; font-size: 12px;\">\r\n                            <p style=\"margin: 0;\">&copy; 2026 BlackBarber. Todos los derechos reservados.</p>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
            
            string url = _config.GetSection("EmailSettings:ResetPasswordUrl").Value;
            url = url + "/" + request.Token;
            string cuerpoFinal = bodyHtml.Replace("{{URL_DE_RESTABLECIMIENTO}}", url);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailSettings:User").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = cuerpoFinal };

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

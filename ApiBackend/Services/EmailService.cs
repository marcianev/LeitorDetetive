using ApiBackend.Configurations;
using ApiBackend.Services.Interface;
using System.Net;
using System.Net.Mail;

namespace ApiBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IConfiguration configuration)
        {
            _settings = configuration
                .GetSection("EmailSettings")
                .Get<EmailSettings>()!;
        }
        
        public async Task<bool> EnviarEmail(string destino, string assunto, string mensagem)
        {
            try
            {
                using var smtp = new SmtpClient(_settings.SmtpServer, _settings.Port)
                {
                    Credentials = new NetworkCredential(_settings.Email, _settings.Senha),
                    EnableSsl = true
                };

                using var email = new MailMessage
                {
                    From = new MailAddress(_settings.Email, "Leitor Detetive"),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = false
                };

                email.To.Add(destino);

                await smtp.SendMailAsync(email);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

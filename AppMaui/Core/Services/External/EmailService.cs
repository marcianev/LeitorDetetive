using AppMaui.Core.Settings;
using System.Net;
using System.Net.Mail;

namespace AppMaui.Core.Services.External
{
    /// <summary>
    /// Serviço para envio de emails usando configurações de SMTP.
    /// </summary>
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        /// <summary>Envia email usando credenciais SMTP configuradas. Retorna sucesso ou falha do envio.</summary>
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

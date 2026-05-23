using AppMaui.Core.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AppMaui.Core.Services.External
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
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
                    From = new MailAddress(_settings.Email, "Ajolede"),
                    Subject = assunto,
                    Body = mensagem,
                    IsBodyHtml = false
                };

                email.To.Add(destino);

                await smtp.SendMailAsync(email);

                return true;
            }
            catch (Exception ex)
            {
                // depois você pode logar isso
                Console.WriteLine($"Erro ao enviar email: {ex.Message}");
                return false;
            }
        }
    }
}

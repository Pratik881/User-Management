using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace UserManagementSystem.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var port = int.Parse(_config["EmailSettings:Port"]);
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderName = _config["EmailSettings:SenderName"];
            var username = _config["EmailSettings:Username"];
            var password = _config["EmailSettings:Password"];
            var enableSsl = bool.Parse(_config["EmailSettings:EnableSsl"]);

            using (var smtpClient = new SmtpClient(smtpServer, port))
            {
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.EnableSsl = enableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}

using Core.Services.Contracts;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace Core.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var useSSL = bool.TryParse(Environment.GetEnvironmentVariable("SMTP_USESSL"), out var result) ? result : true;

            var SmtpClient = new SmtpClient(Environment.GetEnvironmentVariable("SMTP_SERVER"))
            {
                Port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")!),
                Credentials = new NetworkCredential(
                    Environment.GetEnvironmentVariable("SMTP_EMAIL"),
                    Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                    ),
                EnableSsl = useSSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_EMAIL")!),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(to);

            await SmtpClient.SendMailAsync(mailMessage);
        }
    }
}

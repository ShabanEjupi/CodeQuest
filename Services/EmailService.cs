using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace CodeQuest.Services
{
    public interface IEmailService
    {
        Task SendNoReplyEmailAsync(string toEmail, string subject, string body);
        Task SendContactMessageAsync(string fromName, string fromEmail, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpUser = "info@kosovapos.com"; 
        private readonly string _smtpPass;

        public EmailService(IConfiguration configuration)
        {
            // Attempt to get from env first, then config
            _smtpPass = Environment.GetEnvironmentVariable("APP_PASSWORD") ?? configuration["APP_PASSWORD"] ?? string.Empty;
        }

        public async Task SendNoReplyEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(_smtpPass)) return;

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser, "KOSOVAPOS No-Reply"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendContactMessageAsync(string fromName, string fromEmail, string message)
        {
            if (string.IsNullOrEmpty(_smtpPass)) return;

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser, "KOSOVAPOS Website Contact"),
                Subject = $"New Contact Message from {fromName}",
                Body = $"<b>Name:</b> {fromName}<br/><b>Email:</b> {fromEmail}<br/><br/><b>Message:</b><br/>{message}",
                IsBodyHtml = true
            };
            // Send to ourselves
            mailMessage.To.Add(_smtpUser);
            mailMessage.ReplyToList.Add(new MailAddress(fromEmail, fromName));

            await client.SendMailAsync(mailMessage);
        }
    }
}
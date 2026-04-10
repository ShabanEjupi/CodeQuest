using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace CodeQuest.Services
{
    public interface IEmailService
    {
        Task SendNoReplyEmailAsync(string toEmail, string subject, string body, bool isSq = false);
        Task SendContactMessageAsync(string fromName, string fromEmail, string message, bool isSq = false);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpUser = "etinup1@gmail.com"; 
        private readonly string _smtpPass;

        public EmailService(IConfiguration configuration)
        {
            // Attempt to get from env first, then config
            var pass = Environment.GetEnvironmentVariable("APP_PASSWORD") ?? configuration["APP_PASSWORD"] ?? string.Empty;
            // Remove any spaces that might be present in the google app password
            _smtpPass = pass.Replace(" ", "");
        }

        public async Task SendNoReplyEmailAsync(string toEmail, string subject, string body, bool isSq = false)
        {
            if (string.IsNullOrEmpty(_smtpPass)) return;

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser, isSq ? "KOSOVAPOS Mos Kthe Përgjigje" : "KOSOVAPOS No-Reply"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendContactMessageAsync(string fromName, string fromEmail, string message, bool isSq = false)
        {
            if (string.IsNullOrEmpty(_smtpPass)) return;

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpUser, _smtpPass)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser, isSq ? "KOSOVAPOS Kontakt" : "KOSOVAPOS Website Contact"),
                Subject = isSq ? $"Mesazh i Ri Kontakti nga {fromName}" : $"New Contact Message from {fromName}",
                Body = isSq 
                    ? $"<b>Emri:</b> {fromName}<br/><b>Email:</b> {fromEmail}<br/><br/><b>Mesazhi:</b><br/>{message}"
                    : $"<b>Name:</b> {fromName}<br/><b>Email:</b> {fromEmail}<br/><br/><b>Message:</b><br/>{message}",
                IsBodyHtml = true
            };
            // Send to ourselves
            mailMessage.To.Add(_smtpUser);
            mailMessage.ReplyToList.Add(new MailAddress(fromEmail, fromName));

            await client.SendMailAsync(mailMessage);
        }
    }
}
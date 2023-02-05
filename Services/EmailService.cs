using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OttBlog23.Services.Interfaces;
using OttBlog23.ViewModels;
//using Azure.Core;

namespace OttBlog23.Services
{
    public class EmailService : IBlogEmailSender
    {
        private readonly MailSettings _mailSettings;

        //public EmailService(MailSettings mailSettings)
        //{
        //    _mailSettings = mailSettings;
        //}


        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendContactEmailAsync(string email, string name, string subject, string message)
        {
            var newEmail = new MimeMessage();
            newEmail.Sender = MailboxAddress.Parse(_mailSettings.MailEmail);
            newEmail.To.Add(MailboxAddress.Parse(_mailSettings.MailEmail));
            newEmail.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<b>{name}</b> has sent you an new email. and can be reached at: <b>{email}</b><br/><br/>{message}";

            newEmail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.MailHost, _mailSettings.MailPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.MailEmail, _mailSettings.MailPassword);

            await smtp.SendAsync(newEmail);

            smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var newEmail = new MimeMessage();
            newEmail.Sender = MailboxAddress.Parse(_mailSettings.MailEmail);
            newEmail.To.Add(MailboxAddress.Parse(email));
            newEmail.Subject = subject;

            var builder = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            };

            newEmail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.MailHost, _mailSettings.MailPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.MailEmail, _mailSettings.MailPassword);

            await smtp.SendAsync(newEmail);
            smtp.Disconnect(true);
        }
    }
}

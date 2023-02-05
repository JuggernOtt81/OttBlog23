using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using OttBlog23.ViewModels;
using OttBlog23.Services;
using OttBlog23.Services.Interfaces;
using OttBlog23.Models;
using OttBlog23.Data;
using OttBlog23.Data.Migrations;
using OttBlog23.Enums;
using OttBlog23.Helpers;


namespace OttBlog23.Services.Interfaces
{
    public interface IBlogEmailSender : IEmailSender
    {
        //var host = _mailSettings.MailHost ?? Environment.GetEnvironmentVariable("MailHost");
        //var port = _mailSettings.MailPort != 0 ? _mailSettings.MailPort : int.Parse(Environment.GetEnvironmentVariable("MailPort")!);
        //var password = _mailSettings.MailPassword ?? Environment.GetEnvironmentVariable("MailPassword");

        //await smtpClient.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        //await smtpClient.AuthenticateAsync(emailSender, password);
        //await smtpClient.SendAsync(newEmail);
        //await smtpClient.DisconnectAsync(true);
        Task SendContactEmailAsync(string email, string name, string subject, string message);
    }
}

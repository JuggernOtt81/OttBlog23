using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace OttBlog23.Services.Interfaces
{
    public interface IBlogEmailSender : IEmailSender
    {
        Task SendContactEmailAsync(string email, string name, string subject, string message);
    }
}

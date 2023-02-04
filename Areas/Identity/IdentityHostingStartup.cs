[assembly: HostingStartup(typeof(OttBlog23.Areas.Identity.IdentityHostingStartup))]
namespace OttBlog23.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
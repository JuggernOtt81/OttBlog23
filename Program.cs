using OttBlog23.Services;

namespace OttBlog23
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var host = CreateHostBuilder(args).UseContentRoot(Directory.GetCurrentDirectory()).Build();

            var dataService = host.Services.CreateScope().ServiceProvider
                                           .GetRequiredService<DataService>()
                                           .ManageDataAsync();


            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

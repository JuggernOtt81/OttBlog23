using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OttBlog23.Data;
using OttBlog23.Helpers;
using OttBlog23.Models;
using OttBlog23.Services;
using OttBlog23.Services.Interfaces;
using OttBlog23.ViewModels;

namespace OttBlog23
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseNpgsql(
            //        Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(ConnectionHelper.GetConnectionString(Configuration["ConnectionStrings:DefaultConnection"], Environment.GetEnvironmentVariable("DATABASE_URL"))));

            services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultUI()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<DataService>();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<IBlogEmailSender, EmailService>();
            services.AddScoped<IImageService, BasicImageService>();
            services.AddScoped<ISlugService, BasicSlugService>();
            services.AddScoped<BlogSearchService>();
            //services.AddUserSecrets<Startup>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "SlugRoute",
                                pattern: "BlogPosts/UrlFriendly/{slug}",
                                defaults: new { controller = "Posts", action = "Details" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                

                endpoints.MapRazorPages();
            });
        }
    }
}
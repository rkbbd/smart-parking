using Microsoft.Extensions.Configuration;
using SPSApps.Models;
using Microsoft.EntityFrameworkCore;

namespace SPSApps
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseEntity>(options =>
                options.UseSqlServer(configRoot.GetConnectionString("DefaultConnection")));
            services.AddRazorPages();
            //services.AddControllersWithViews();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Create}");
            app.MapRazorPages();
            app.Run();
        }
    }
}

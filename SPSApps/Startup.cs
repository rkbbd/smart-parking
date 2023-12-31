﻿using Microsoft.Extensions.Configuration;
using SPSApps.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            services.AddMvc()
       .AddSessionStateTempDataProvider();
            services.AddSession();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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
            app.UseSession();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}");
            app.MapRazorPages();
            app.Run();
        }
    }
}

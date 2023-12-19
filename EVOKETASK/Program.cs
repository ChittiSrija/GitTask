using EVOKETASK.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EVOKETASK
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
        builder.Services.AddHostedService<BackgroundTask>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.Cookie.HttpOnly = true;
           options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
           options.LoginPath = "/Home/Login";
           options.SlidingExpiration = true;
       });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireLoggedIn", policy =>
                policy.RequireAuthenticatedUser());
        });
        builder.Services.Configure<CookieAuthenticationOptions>(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10); // Set expiration time
            options.SlidingExpiration = true; // Extend the expiration time on activity
        });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
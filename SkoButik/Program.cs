using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkoButik.Data;
using SkoButik.Data.Cart;
using SkoButik.Data.Services;
using SkoButik.Models;
using SkoButik.Utility;

namespace SkoButik
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services configuration
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));
            builder.Services.AddSession();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddControllersWithViews();
            builder.Services.ConfigureApplicationCookie(option =>
            {
                option.LoginPath = $"/Identity/account/Login";
                option.LogoutPath = $"/Identity/account/Logout";
                option.AccessDeniedPath = $"/Identity/account/AccessDenied";
            });
            builder.Services.AddRazorPages();

            var app = builder.Build();

            //seed database
            AppDbInitializer.Seed(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();
            //Useession
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();

            //Testar att pusha
 
        }
    }
}

using Microsoft.EntityFrameworkCore;
using GuestHouseRoomsTracker.DataAccess;
using GuestHouseRoomsTracker.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using GuestHouseRoomsTracker.Areas.Identity.Pages.Account.Manage;
using GuestHouseRoomsTracker.Utility;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        var connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("GuestHouseRoomsTracker.DataAccess")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
 
        builder.Services.AddRazorPages();

        builder.Services.AddIdentity<User, IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders(); builder.Services.AddScoped<IEmailSender, EmailSender>();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            await RoleSeeder.Initialize(serviceProvider);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        
        app.UseAuthentication();
        
        app.UseAuthorization();
        
        app.MapRazorPages();
        
        app.UseStaticFiles();
        
        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
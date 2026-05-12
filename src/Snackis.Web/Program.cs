using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snackis.Infrastructure.Data;
using Snackis.Web.Data;
using Snackis.Domain.Entities;

namespace Snackis.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Identity DbContext 
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // forum‑databas
            builder.Services.AddDbContext<SnackisDbContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddDefaultIdentity<AppUser>(options =>
               options.SignIn.RequireConfirmedAccount = true)
               .AddRoles<IdentityRole>() 
               .AddEntityFrameworkStores<ApplicationDbContext>();



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ShouldBeAdmin", policy => policy.RequireRole("Admin"));
            });

            

            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            // Seed
            using (var scope = app.Services.CreateScope())
            {
                await SeedData.Initialize(scope.ServiceProvider);
            }
            
            app.Run();
        }

    }
}
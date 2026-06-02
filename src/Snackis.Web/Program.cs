using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snackis.Application.Services;
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using Snackis.Infrastructure.Data;
using Snackis.Infrastructure.Repositories;
using Snackis.Web.Data;

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


            // Repositories
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IComentRepository, ComentRepository>();
            builder.Services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();

            // Services
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IComentService, ComentService>();
            builder.Services.AddScoped<IPrivateMessageService, PrivateMessageService>();
            builder.Services.AddScoped<IReportService, ReportService>();


            builder.Services.AddDefaultIdentity<AppUser>(options =>
               options.SignIn.RequireConfirmedAccount = false) // false för att loggar in direkt utan bekräfta
               .AddRoles<IdentityRole>() 
               .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ShouldBeAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ShouldBeUser", policy => policy.RequireRole("User"));
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
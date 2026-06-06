using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snackis.Application.HttpServices;
using Snackis.Application.Services;
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using Snackis.Infrastructure.Data;
using Snackis.Infrastructure.Repositories;
using Snackis.Web.Data;
using Snackis.Web.HttpServices;

namespace Snackis.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


            // forum‑databas
            builder.Services.AddDbContext<SnackisDbContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddHttpClient<IPostServiceApi, PostServiceApi>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]
                    ?? throw new InvalidOperationException("ApiSettings:BaseUrl saknas i appsettings.json"));
            });

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

            // Identity
            builder.Services.AddDefaultIdentity<AppUser>(options =>
               options.SignIn.RequireConfirmedAccount = false) // false för att loggar in direkt utan bekräfta
               .AddRoles<IdentityRole>() 
               .AddEntityFrameworkStores<SnackisDbContext>(); 

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ShouldBeAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ShouldBeUser", policy => policy.RequireRole("User"));
            });

            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

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
            app.MapControllers();
            // Seed
            using (var scope = app.Services.CreateScope())
            {
                await SeedData.Initialize(scope.ServiceProvider);
            }
            
            app.Run();
        }

    }
}
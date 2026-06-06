
using Microsoft.EntityFrameworkCore;
using Snackis.Application.Services;
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using Snackis.Infrastructure.Data;
using Snackis.Infrastructure.Repositories;

namespace Snackis.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = WebApplication.CreateBuilder(args);

            //// Add services to the container.


            //builder.Services.AddDbContext<SnackisDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddIdentityCore<AppUser>()
            //   .AddEntityFrameworkStores<SnackisDbContext>();

            //builder.Services.AddScoped<IPostRepository, PostRepository>();
            //builder.Services.AddScoped<IPostService, PostService>();


            //builder.Services.AddControllers();
            //// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            //var app = builder.Build();

            //// Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();
            //app.UseAuthorization();
            //app.MapControllers();
            //app.Run();
        }
    }
}

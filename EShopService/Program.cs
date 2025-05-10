
using EShop.Application.Services;
using EShop.Domain.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Seeders;

namespace EShopService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Konfiguracja kontekstu bazy danych
            builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDatabase"), ServiceLifetime.Transient);
            builder.Services.AddScoped<IRepository, Repository>();


            // Add services to the container.
            builder.Services.AddScoped<ICreditCardService, CreditCardService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IEShopSeeder, EShopSeeder>();





            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();



            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IEShopSeeder>();
            seeder.Seed();


            app.Run();
        }
    }
}

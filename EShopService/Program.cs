
using EShop.Application.Services;
using EShop.Domain.Repositories;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using EShop.Domain.Seeders;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

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


            //memory cache
            builder.Services.AddMemoryCache();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EShop API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Wpisz token w formacie: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "data", "public.key")));
                var publicKey = new RsaSecurityKey(rsa);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "EShopService",
                    ValidAudience = "EShop",
                    IssuerSigningKey = publicKey
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Administrator"));
                options.AddPolicy("UserOnly", policy =>
                    policy.RequireRole("Employee"));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();



            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IEShopSeeder>();
            seeder.Seed();


            app.Run();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;
using EShop.Domain.Repositories;
using EShopService.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace EShopService.IntegrationTests.Controllers;

public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // pobranie dotychczasowej konfiguracji bazy danych
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataContext>));

                    //// usunięcie dotychczasowej konfiguracji bazy danych
                    services.Remove(dbContextOptions);

                    // Stworzenie nowej bazy danych
                    services
                        .AddDbContext<DataContext>(options => options.UseInMemoryDatabase("MyDBForTest"));

                });
            });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsAllProducts_ExpectedTwoProducts()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            // Pobranie kontekstu bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Products.RemoveRange(dbContext.Products);

            // Stworzenie obiektu
            dbContext.Products.AddRange(
                new Product { Name = "Product1" },
                new Product { Name = "Product2" }
            );
            // Zapisanie obiektu
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.Equal(2, products?.Count);
    }

    [Fact]
    public async Task Post_AddTenThousandProductsAsync_ExpectedTenThousandProducts()
    // tak naprawde nie testuję posta tylko get'a a test jest na asynchronicznym dodawaniu elementó do DBContextu
    {     
    // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            // Pobranie kontekstu bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Products.RemoveRange(dbContext.Products);

            // Stworzenie 1000 obiektów

            for (int i = 0; i < 10000; i++)
            {
                dbContext.Products.Add(new Product { Name = $"Product{i}" });
                // Zapisanie obiektu
                await dbContext.SaveChangesAsync();
            }

        }

        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.Equal(10000, products?.Count);
    }


    [Fact]
    public async Task Post_AddTenThousandProducts_ExpectedTenThousandProducts()
    // tak naprawde nie testuję posta tylko get'a a test jest na synchronicznym dodawaniu elementów do DBContextu
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            // Pobranie kontekstu bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Products.RemoveRange(dbContext.Products);

            // Stworzenie 1000 obiektów

            for (int i = 0; i < 10000; i++)
            {
                dbContext.Products.Add(new Product { Name = $"Product{i}" });
                // Zapisanie obiektu
                dbContext.SaveChangesAsync();
            }

        }

        // Act
        var response = await _client.GetAsync("/api/product");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.Equal(10000, products?.Count);
    }


    [Fact]
    public async Task Add_AddProduct_ExpectedOneProduct()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            // Pobranie kontekstu bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            dbContext.Products.RemoveRange(dbContext.Products);
            dbContext.SaveChanges();

            // Act

            //tworze product
            var product = new Product { Name = "Product1", Category = new Category { Name = "Category1" } };

            var jsonContent = JsonConvert.SerializeObject(product);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PatchAsync("/api/product", content);

            var result = await dbContext.Products.ToListAsync();

            // Assert

            Assert.Equal(1, result?.Count);
        }
    }
}
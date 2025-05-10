using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Repositories;
using EShop.Domain.Models;

namespace EShop.Domain.Seeders;

public class EShopSeeder(DataContext context) : IEShopSeeder
{

    public async Task Seed()
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Name = "Underware" },
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

            if (!context.Products.Any())
        {
            var products = new List<Product>
            {
                new Product { Name = "Boxers", Ean = "111" },
                new Product { Name = "Thongs", Ean = "222" },
                new Product { Name = "Strings", Ean = "333" }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}

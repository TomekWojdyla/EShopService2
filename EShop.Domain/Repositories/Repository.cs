using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Domain.Repositories;

public class Repository : IRepository
{
    //z jakiego kontekstu korzysta repozytorium
    private readonly DataContext _context;

    //konstruktor repozytorium
    public Repository(DataContext context)
    {
        _context = context;
    }
    public async Task<Product> AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<List<Product>> GetAllProductAsync()
    {
        return await _context.Products.ToListAsync();
    }


    public async Task<Product> GetProductAsync(int id)
    {
        return await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }
}

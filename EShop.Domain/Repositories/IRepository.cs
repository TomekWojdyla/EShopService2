using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;

namespace EShop.Domain.Repositories;

public interface IRepository
{
    #region Product
    Task<Product> GetProductAsync(int id);
    Task<Product> AddProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product user);
    Task<List<Product>> GetAllProductAsync();
    #endregion
}

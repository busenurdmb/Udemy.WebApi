using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udemy.WebApi.Data;
using Udemy.WebApi.Interfaces;

namespace Udemy.WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _productContext;

        public ProductRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public async Task<Product> CreateAsync(Product product)
        {
          await _productContext.Products.AddAsync(product);
            await _productContext.SaveChangesAsync();
            return product;
        }

        public  async Task<List<Product>> GetAllAsync()
        {
            return await _productContext.Products.AsNoTracking().ToListAsync();
          
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productContext.Products.AsNoTracking().SingleOrDefaultAsync(x=>x.Id==id);
        }

        public async Task RemoveAsync(int id)
        {
            var removedEntity = await _productContext.Products.FindAsync(id);
            _productContext.Products.Remove(removedEntity);
            await _productContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            var unchangedEntity = await _productContext.Products.FindAsync(product.Id);
            _productContext.Entry(unchangedEntity).CurrentValues.SetValues(product);
           await  _productContext.SaveChangesAsync();
        }
    }
}

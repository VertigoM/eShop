using eShop.API.Entities.Models;

namespace eShop.API.Services
{
    public class ProductsService
    {
        private readonly DataDbContext _context;

        public ProductsService(DataDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product Product)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(int id, Product Product)
        {
            throw new NotImplementedException();
        }
    }
}

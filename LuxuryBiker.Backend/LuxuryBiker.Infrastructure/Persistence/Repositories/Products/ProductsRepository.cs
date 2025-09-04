using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace LuxuryBiker.Infrastructure.Persistence.Repositories.Products
{
    internal class ProductsRepository : IProductsRepository
    {
        private readonly LuxuryBikerDbContext _context;
        public ProductsRepository(LuxuryBikerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException();
        }

        public Task CreateAsync(Product entity)
        {
            _context.Products.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Product>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetAsync(int entityId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product?> GetByReference(string reference)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Reference.ToUpper().Equals(reference.ToUpper()));
        }

        public Task UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
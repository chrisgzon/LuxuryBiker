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
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task CreateAsync(Product entity)
        {
            _context.Products.Add(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetAsync(int entityId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == entityId);
        }

        public async Task<Product?> GetByReference(string reference)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Reference.ToUpper().Equals(reference.ToUpper()));
        }

        public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, bool onlyActive, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _context.Products.AsNoTracking();

            if (onlyActive)
            {
                query = query.Where(p => p.Status == true);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            List<Product> items = await query
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<IReadOnlyList<Product>> GetByIdsAsync(
            IEnumerable<int> ids, CancellationToken cancellationToken)
        {
            var idSet = ids.ToList();
            // Rastreados a propósito: el handler de compra muta stock/valor y esos
            // cambios deben confirmarse junto con la compra.
            return await _context.Products
                .Where(p => idSet.Contains(p.Id))
                .ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(Product entity)
        {
            _context.Products.Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}

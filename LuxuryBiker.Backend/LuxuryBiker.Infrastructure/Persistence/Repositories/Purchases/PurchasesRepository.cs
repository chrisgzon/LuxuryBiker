using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Repositories.Purchases;
using Microsoft.EntityFrameworkCore;

namespace LuxuryBiker.Infrastructure.Persistence.Repositories.Purchases
{
    internal class PurchasesRepository : IPurchasesRepository
    {
        private readonly LuxuryBikerDbContext _context;

        public PurchasesRepository(LuxuryBikerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string?> GetLastCodeAsync(CancellationToken cancellationToken)
        {
            return await _context.Purchases
                .OrderByDescending(p => p.Id)
                .Select(p => p.Code)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task CreateAsync(Purchase purchase, CancellationToken cancellationToken)
        {
            _context.Purchases.Add(purchase);
            // Un único SaveChanges confirma la compra y los cambios de stock/valor de los
            // productos que el handler dejó rastreados en el mismo DbContext (atómico).
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Purchase?> GetByIdWithDetailsAsync(int purchaseId, CancellationToken cancellationToken)
        {
            return await _context.Purchases
                .Include(p => p.Details)
                .FirstOrDefaultAsync(p => p.Id == purchaseId, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<Purchase> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, DateTimeOffset? dateFrom, DateTimeOffset? dateTo,
            CancellationToken cancellationToken)
        {
            IQueryable<Purchase> query = _context.Purchases
                .AsNoTracking()
                .Include(p => p.Third)
                .Include(p => p.Details);

            if (dateFrom.HasValue)
            {
                query = query.Where(p => p.DatePurchase >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(p => p.DatePurchase <= dateTo.Value);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            List<Purchase> items = await query
                .OrderByDescending(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}

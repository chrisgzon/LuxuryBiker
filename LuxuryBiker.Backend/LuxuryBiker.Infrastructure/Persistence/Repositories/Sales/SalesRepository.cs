using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Repositories.Sales;
using Microsoft.EntityFrameworkCore;

namespace LuxuryBiker.Infrastructure.Persistence.Repositories.Sales
{
    internal class SalesRepository : ISalesRepository
    {
        private readonly LuxuryBikerDbContext _context;

        public SalesRepository(LuxuryBikerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string?> GetLastCodeAsync(CancellationToken cancellationToken)
        {
            return await _context.Sales
                .OrderByDescending(s => s.Id)
                .Select(s => s.Code)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task CreateAsync(Sale sale, CancellationToken cancellationToken)
        {
            _context.Sales.Add(sale);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Sale?> GetByIdWithDetailsAsync(int saleId, CancellationToken cancellationToken)
        {
            return await _context.Sales
                .Include(s => s.Details)
                .FirstOrDefaultAsync(s => s.Id == saleId, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<Sale> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, DateTimeOffset? dateFrom, DateTimeOffset? dateTo,
            CancellationToken cancellationToken)
        {
            IQueryable<Sale> query = _context.Sales
                .AsNoTracking()
                .Include(s => s.Third)
                .Include(s => s.Details);

            if (dateFrom.HasValue)
            {
                query = query.Where(s => s.Date >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(s => s.Date <= dateTo.Value);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            List<Sale> items = await query
                .OrderByDescending(s => s.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}

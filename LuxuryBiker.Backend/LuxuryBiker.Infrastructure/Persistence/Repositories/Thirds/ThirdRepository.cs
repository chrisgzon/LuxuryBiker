using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Thirds;
using Microsoft.EntityFrameworkCore;
using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Infrastructure.Persistence.Repositories.Thirds
{
    internal class ThirdRepository : IThirdRepository
    {
        private readonly LuxuryBikerDbContext _context;
        public ThirdRepository(LuxuryBikerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task CreateAsync(Third entity)
        {
            _context.Add(entity);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Third>> GetAsync()
        {
            return await _context.Thirds.AsNoTracking().Include(t => t.Type).ToListAsync();
        }

        public async Task<Third?> GetAsync(int entityId)
        {
            return await _context.Thirds.FirstOrDefaultAsync(t => t.Id.Equals(entityId));
        }

        public Task<Third?> GetByIdentification(string identification, int typeID)
        {
            return _context.Thirds.FirstOrDefaultAsync(t => t.Identification.Equals(identification) && t.TypeId.Equals(typeID));
        }

        public async Task<Page<Third>> GetPagedAsync(
            int pageNumber, int pageSize, int? typeId, CancellationToken cancellationToken)
        {
            IQueryable<Third> query = _context.Thirds.AsNoTracking().Include(t => t.Type);

            if (typeId.HasValue)
            {
                query = query.Where(t => t.TypeId == typeId.Value);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            List<Third> items = await query
                .OrderByDescending(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new Page<Third>(items, totalCount);
        }

        public Task UpdateAsync(Third entity)
        {
            _context.Thirds.Update(entity);
            return _context.SaveChangesAsync();
        }
    }
}

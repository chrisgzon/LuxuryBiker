using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Thirds;
using Microsoft.EntityFrameworkCore;

namespace LuxuryBiker.Infrastructure.Persistence.Repositories.Thirds
{
    internal class ThirdRepository : IThirdRepository
    {
        private readonly LuxuryBikerDbContext _context;
        public ThirdRepository(LuxuryBikerDbContext context)
        {
            _context = context ?? throw new ArgumentNullException();
        }

        public Task CreateAsync(Third entity)
        {
            _context.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Third>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Third?> GetAsync(int entityId)
        {
            return await _context.Thirds.FirstOrDefaultAsync(t => t.Id.Equals(entityId));
        }

        public Task<Third?> GetByIdentification(string identification)
        {
            return _context.Thirds.FirstOrDefaultAsync(t =>  t.Identification != null && t.Identification.Equals(identification));
        }

        public Task UpdateAsync(Third entity)
        {
            throw new NotImplementedException();
        }        
    }
}

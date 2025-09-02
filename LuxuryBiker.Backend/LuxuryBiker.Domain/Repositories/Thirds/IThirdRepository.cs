using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Common;

namespace LuxuryBiker.Domain.Repositories.Thirds
{
    public interface IThirdRepository : IRepositoryBase<Third, int>
    {
        Task<Third?> GetByIdentification(string identification);
    }
}

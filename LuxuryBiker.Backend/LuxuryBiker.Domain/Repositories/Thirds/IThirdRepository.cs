using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Common;
using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Domain.Repositories.Thirds
{
    public interface IThirdRepository : IRepositoryBase<Third, int>
    {
        Task<Third?> GetByIdentification(string identification, int typeID);

        Task<Page<Third>> GetPagedAsync(
            int pageNumber, int pageSize, int? typeId, CancellationToken cancellationToken);
    }
}

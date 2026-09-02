using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Common;

namespace LuxuryBiker.Domain.Repositories.Thirds
{
    public interface IThirdRepository : IRepositoryBase<Third, int>
    {
        Task<Third?> GetByIdentification(string identification, int typeID);

        Task<(IReadOnlyList<Third> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, int? typeId, CancellationToken cancellationToken);
    }
}

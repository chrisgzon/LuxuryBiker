using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Domain.Repositories.Sales
{
    public interface ISalesRepository
    {
        Task<string?> GetLastCodeAsync(CancellationToken cancellationToken);

        Task CreateAsync(Sale sale, CancellationToken cancellationToken);

        /// <summary>Venta con sus líneas, rastreada por el contexto (para cambiar estado).</summary>
        Task<Sale?> GetByIdWithDetailsAsync(int saleId, CancellationToken cancellationToken);

        /// <summary>Confirma cambios pendientes del contexto (venta + ajustes de stock).</summary>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task<Page<Sale>> GetPagedAsync(
            int pageNumber, int pageSize, DateTimeOffset? dateFrom, DateTimeOffset? dateTo,
            CancellationToken cancellationToken);
    }
}

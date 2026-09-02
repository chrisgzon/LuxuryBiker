using LuxuryBiker.Domain.Entities.Sales;

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

        Task<(IReadOnlyList<Sale> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, DateTimeOffset? dateFrom, DateTimeOffset? dateTo,
            CancellationToken cancellationToken);
    }
}

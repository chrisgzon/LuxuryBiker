using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Domain.Repositories.Purchases
{
    public interface IPurchasesRepository
    {
        /// <summary>Código de la última compra registrada (para generar el siguiente).</summary>
        Task<string?> GetLastCodeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Persiste la compra y confirma en la misma unidad de trabajo cualquier cambio
        /// pendiente en el <c>DbContext</c> (p. ej. ajustes de stock de productos).
        /// </summary>
        Task CreateAsync(Purchase purchase, CancellationToken cancellationToken);

        /// <summary>Compra con sus líneas, rastreada por el contexto (para cambiar estado).</summary>
        Task<Purchase?> GetByIdWithDetailsAsync(int purchaseId, CancellationToken cancellationToken);

        /// <summary>Confirma cambios pendientes del contexto (compra + ajustes de stock).</summary>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task<Page<Purchase>> GetPagedAsync(
            int pageNumber, int pageSize, DateTimeOffset? dateFrom, DateTimeOffset? dateTo,
            CancellationToken cancellationToken);
    }
}

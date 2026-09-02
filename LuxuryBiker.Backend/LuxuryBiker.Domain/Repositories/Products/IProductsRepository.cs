using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Common;

namespace LuxuryBiker.Domain.Repositories.Products
{
    public interface IProductsRepository : IRepositoryBase<Product, int>
    {
        Task<Product?> GetByReference(string reference);

        Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, bool onlyActive, CancellationToken cancellationToken);

        /// <summary>
        /// Devuelve los productos indicados <b>rastreados por el contexto</b>, para que
        /// los cambios de stock/valor se confirmen junto con la compra que los origina.
        /// </summary>
        Task<IReadOnlyList<Product>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
    }
}

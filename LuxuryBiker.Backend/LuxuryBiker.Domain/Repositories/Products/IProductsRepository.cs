using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Common;
using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Domain.Repositories.Products
{
    public interface IProductsRepository : IRepositoryBase<Product, int>
    {
        Task<Product?> GetByReference(string reference);

        Task<Page<Product>> GetPagedAsync(
            int pageNumber, int pageSize, bool onlyActive, CancellationToken cancellationToken);

        /// <summary>
        /// Carga los productos indicados con intención de modificarlos: los cambios que
        /// se les apliquen se persisten junto con la operación que los origina.
        /// </summary>
        Task<IReadOnlyList<Product>> GetForUpdateAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
    }
}

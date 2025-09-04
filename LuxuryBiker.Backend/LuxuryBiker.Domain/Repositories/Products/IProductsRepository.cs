using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Common;

namespace LuxuryBiker.Domain.Repositories.Products
{
    public interface IProductsRepository : IRepositoryBase<Product, int>
    {
        Task<Product?> GetByReference(string reference);
    }
}

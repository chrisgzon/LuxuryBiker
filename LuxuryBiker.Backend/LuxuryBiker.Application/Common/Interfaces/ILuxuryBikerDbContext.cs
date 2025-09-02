using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Entities.Thirds;
using Microsoft.EntityFrameworkCore;

namespace LuxuryBiker.Application.Common.Interfaces
{
    public interface ILuxuryBikerDbContext
    {
        #region Purchases
        public DbSet<Purchase>   Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        #endregion
        #region Products
        public DbSet<Product> Products { get; set; }
        #endregion
        #region Thirds
        public DbSet<Third> Thirds { get; set; }
        public DbSet<TypeThird> TypeThird { get; set; }
        #endregion
        #region Sales
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}

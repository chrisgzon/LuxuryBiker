using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Infrastructure.Identity;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Identity;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Products;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Purchases;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Sales;
using LuxuryBiker.Infrastructure.Persistence.Metadata.Thirds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LuxuryBiker.Infrastructure.Persistence
{
    public class LuxuryBikerDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Purchases
        public DbSet<Purchase> Purchases { get; set; } = null!;
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; } = null!;
        #endregion
        #region Products
        public DbSet<Product> Products { get; set; } = null!;
        #endregion
        #region Thirds
        public DbSet<Third> Thirds { get; set; } = null!;
        public DbSet<TypeThird> TypeThird { get; set; } = null!;
        #endregion
        #region Sales
        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<SaleDetail> SaleDetails { get; set; } = null!;
        #endregion

        public LuxuryBikerDbContext(DbContextOptions<LuxuryBikerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Thirds
            ThirdsMetadata.SetEntityBuilder(builder.Entity<Third>());
            ThirdTypeMetadata.SetEntityBuilder(builder.Entity<TypeThird>());
            #endregion
            #region purchases
            PurchasesMetadata.SetEntityBuilder(builder.Entity<Purchase>());
            PurchaseDetailsMetadata.SetEntityBuilder(builder.Entity<PurchaseDetail>());
            #endregion
            #region sales
            SalesMetadata.SetEntityBuilder(builder.Entity<Sale>());
            SaleDetailsMetadata.SetEntityBuilder(builder.Entity<SaleDetail>());
            #endregion
            #region Products
            ProductsMetadata.SetEntityBuilder(builder.Entity<Product>());
            #endregion
            #region Identity
            ApplicationUserMetadata.SetEntityBuilder(builder.Entity<ApplicationUser>());
            #endregion

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

    }
}

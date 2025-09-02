using LuxuryBiker.Domain.Entities.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Purchases
{
    internal class PurchaseDetailsMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<PurchaseDetail> entityBuilder)
        {
            entityBuilder.ToTable("T_PURCHASE_DETAILS").HasKey(x => x.Id);
            entityBuilder.Property(x => x.Quantity).IsRequired().HasPrecision(10, 3);
            entityBuilder.Property(x => x.ProductId).IsRequired();
            entityBuilder.Property(x => x.ProductId).IsRequired();
            entityBuilder.Property(x => x.ProductValue).IsRequired().HasPrecision(28, 6);

            entityBuilder.HasOne(x => x.Purchase).WithMany(x => x.Details).HasForeignKey(x => x.PurchaseId).IsRequired();
            entityBuilder.HasOne(x => x.Product).WithMany(x => x.Purchases).HasForeignKey(x => x.ProductId).IsRequired();
        }
    }
}
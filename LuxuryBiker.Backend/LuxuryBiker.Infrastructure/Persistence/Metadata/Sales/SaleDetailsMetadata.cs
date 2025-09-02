using LuxuryBiker.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Sales
{
    internal class SaleDetailsMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<SaleDetail> entityBuilder)
        {
            entityBuilder.ToTable("T_SALE_DETAILS").HasKey(x => x.Id);
            entityBuilder.Property(x => x.Quantity).IsRequired().HasPrecision(10, 3);
            entityBuilder.Property(x => x.SaleId).IsRequired();
            entityBuilder.Property(x => x.ProductId).IsRequired();
            entityBuilder.Property(x => x.ProductValue).IsRequired().HasPrecision(28, 2);

            entityBuilder.HasOne(x => x.Sale).WithMany(x => x.Details).HasForeignKey(x => x.SaleId).IsRequired();
            entityBuilder.HasOne(x => x.Product).WithMany(x => x.Sales).HasForeignKey(x => x.ProductId).IsRequired();
        }
    }
}

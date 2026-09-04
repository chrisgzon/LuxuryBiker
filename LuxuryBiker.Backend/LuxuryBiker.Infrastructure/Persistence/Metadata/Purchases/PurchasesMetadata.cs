using LuxuryBiker.Domain.Entities.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Purchases
{
    internal class PurchasesMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Purchase> entityBuilder)
        {
            // In this table are registered the purchases than make the company for fill their inventory
            entityBuilder.ToTable("T_PURCHASES").HasKey(x => x.Id);
            entityBuilder.Property(x => x.Code).HasMaxLength(60).IsRequired();
            entityBuilder.Property(x => x.Status).IsRequired().HasDefaultValue(true);
            entityBuilder.Property(x => x.DatePurchase).IsRequired();
            entityBuilder.Property(x => x.UserId).IsRequired();
            entityBuilder.Property(x => x.ThirdId).IsRequired(false);
            entityBuilder.Property(x => x.Total).HasPrecision(28, 2);

            entityBuilder.HasOne(x => x.Third).WithMany(x => x.Sales).HasForeignKey(x => x.ThirdId).IsRequired(false);
        }
    }
}

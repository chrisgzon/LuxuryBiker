using LuxuryBiker.Domain.Entities.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Sales
{
    internal class SalesMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Sale> entityBuilder)
        {
            // In this table are registered the purchases than make the clients to the company
            entityBuilder.ToTable("T_SALES").HasKey(x => x.Id);
            entityBuilder.Property(x => x.Code).HasMaxLength(60).IsRequired();
            entityBuilder.Property(x => x.Status).IsRequired().HasDefaultValue(true);
            entityBuilder.Property(x => x.Date).IsRequired();
            entityBuilder.Property(x => x.UserId).IsRequired();
            entityBuilder.Property(x => x.ThirdId).IsRequired(false);
            entityBuilder.Property(x => x.Total).HasPrecision(28, 2);

            entityBuilder.HasOne(x => x.Third).WithMany(x => x.Purchases).HasForeignKey(x => x.ThirdId).IsRequired(false);
        }
    }
}

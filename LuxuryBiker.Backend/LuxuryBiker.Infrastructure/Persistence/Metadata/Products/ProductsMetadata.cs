using LuxuryBiker.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Products
{
    internal class ProductsMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Product> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("T_PRODUCTS").HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Code).HasMaxLength(60).IsRequired();
            entityTypeBuilder.Property(x => x.Status).IsRequired().HasDefaultValue(true);
            entityTypeBuilder.Property(x => x.Created).IsRequired();
            entityTypeBuilder.Property(x => x.Name).IsRequired().HasMaxLength(60);
            entityTypeBuilder.Property(x => x.Description).HasMaxLength(500);
            entityTypeBuilder.Property(x => x.Stock).HasPrecision(10, 2);
            entityTypeBuilder.Property(x => x.Value).HasPrecision(28, 2);
        }
    }
}

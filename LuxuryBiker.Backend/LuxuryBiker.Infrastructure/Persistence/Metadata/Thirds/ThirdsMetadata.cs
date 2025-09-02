using LuxuryBiker.Domain.Entities.Thirds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Thirds
{
    internal class ThirdsMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Third> entityBuilder)
        {
            entityBuilder.ToTable("T_THIRDS").HasKey(x => x.Id);

            entityBuilder.Property(x => x.Address).HasMaxLength(200);
            entityBuilder.Property(x => x.Email).HasMaxLength(150);
            entityBuilder.Property(x => x.Created).IsRequired();
            entityBuilder.Property(x => x.Identification).HasMaxLength(60).IsRequired();
            entityBuilder.Property(x => x.TypeId).IsRequired();
            entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            entityBuilder.Property(x => x.Surnames).HasMaxLength(250);
            entityBuilder.Property(x => x.CellPhone).HasMaxLength(50);
            entityBuilder.Property(x => x.Active).IsRequired().HasDefaultValue(true);

            entityBuilder.HasOne(x => x.Type).WithMany(x => x.Thirds).HasForeignKey(x => x.TypeId).IsRequired();
        }
    }
}

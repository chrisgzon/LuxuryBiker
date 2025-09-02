using LuxuryBiker.Domain.Entities.Thirds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Thirds
{
    internal class ThirdTypeMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<TypeThird> entityBuilder)
        {
            entityBuilder.ToTable("T_TYPE_THIRD").HasKey(x => x.Id);
        }
    }
}

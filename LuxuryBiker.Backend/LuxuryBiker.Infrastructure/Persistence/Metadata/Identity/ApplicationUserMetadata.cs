using LuxuryBiker.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuxuryBiker.Infrastructure.Persistence.Metadata.Identity
{
    internal class ApplicationUserMetadata
    {
        /// <summary>
        /// La relación usuario -> compras/ventas se configura desde el lado de Identity:
        /// el dominio solo guarda el <c>UserId</c> y no tiene navegación al usuario.
        /// El esquema resultante es idéntico al anterior.
        /// </summary>
        public static void SetEntityBuilder(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            entityBuilder
                .HasMany(x => x.Purchases)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            entityBuilder
                .HasMany(x => x.Sales)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}

using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;
using Microsoft.AspNetCore.Identity;

namespace LuxuryBiker.Infrastructure.Identity
{
    /// <summary>
    /// Usuario de ASP.NET Identity. Vive en Infrastructure porque hereda de
    /// <see cref="IdentityUser"/>: la identidad es un detalle de infraestructura y el
    /// dominio solo conoce el <c>UserId</c> que guarda en compras y ventas.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string Names { get; set; } = string.Empty;
        public string Surnames { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTimeOffset? DateBirth { get; set; }

        public ICollection<Purchase>? Purchases { get; set; }
        public ICollection<Sale>? Sales { get; set; }
    }
}

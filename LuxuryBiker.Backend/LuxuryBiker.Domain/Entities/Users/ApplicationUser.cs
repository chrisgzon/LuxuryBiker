using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;
using Microsoft.AspNetCore.Identity;

namespace LuxuryBiker.Domain.Entities.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string Names { get; set; } = string.Empty;
        public string Surnames { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTimeOffset? DateBirth { get; set; }
        public IEnumerable<Purchase>? Purchases { get; set; }
        public IEnumerable<Sale>? Sales { get; set; }
    }
}
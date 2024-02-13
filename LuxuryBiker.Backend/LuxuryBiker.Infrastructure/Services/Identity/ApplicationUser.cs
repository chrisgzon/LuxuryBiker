using Microsoft.AspNetCore.Identity;

namespace LuxuryBiker.Infrastructure.Services.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Names { get; set; }
        public string? Surnames { get; set; }
        public string? Identification { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset? FechaNacimiento { get; set; }
    }
}
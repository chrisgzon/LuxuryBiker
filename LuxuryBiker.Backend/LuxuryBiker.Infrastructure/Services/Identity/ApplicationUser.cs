using Microsoft.AspNetCore.Identity;

namespace LuxuryBiker.Infrastructure.Services.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Names { get; set; } = string.Empty;
        public string Surnames { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTimeOffset? FechaNacimiento { get; set; }
    }
}
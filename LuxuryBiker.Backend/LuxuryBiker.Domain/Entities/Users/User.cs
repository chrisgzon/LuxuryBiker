using LuxuryBiker.Domain.Entities.Common;

namespace LuxuryBiker.Domain.Entities.Users
{
    public class User : BaseAuditableEntity<Guid>
    {
        public string? Names { get; set; }
        public string? Surnames { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Identification {get;set;}
        public bool Active { get; set; }
        public DateTimeOffset? FechaNacimiento { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

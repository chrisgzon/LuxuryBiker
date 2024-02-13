using LuxuryBiker.Infrastructure.Services.Identity;

namespace LuxuryBiker.Infrastructure.Services.Authentication
{
    public class ApplicationUserDTO : ApplicationUser
    {
        public string Token { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
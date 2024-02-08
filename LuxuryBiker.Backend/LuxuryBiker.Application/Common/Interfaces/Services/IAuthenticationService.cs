using LuxuryBiker.Application.Common.Models;

namespace LuxuryBiker.Application.Common.Interfaces.Services
{
    public interface IAuthenticationService
    {
        string Authenticate(Guid userId, string email, bool rememberme);
        bool CheckPassword(string hashedPassword, string providedPassword);
    }
}
using LuxuryBiker.Application.Common.Models;

namespace LuxuryBiker.Application.Common.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<ErrorOr<string>> Authenticate(string username, string password, bool rememberMe);
        Task<ErrorOr<AuthenticatedUserDto>> GetCurrentUserProfile();
    }
}

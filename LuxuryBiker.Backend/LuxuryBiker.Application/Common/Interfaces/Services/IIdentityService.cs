using LuxuryBiker.Application.Common.Models;

namespace LuxuryBiker.Application.Common.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string role);

        Task<bool> AuthorizeAsync(Guid userId, string policyName);

        Task<(Response Result, Guid UserId)> CreateUserAsync(string userName, string password);

        Task<Response> DeleteUserAsync(Guid userId);
    }
}
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Application.Common.Models;

namespace LuxuryBiker.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    public Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        throw new NotImplementedException();
    }

    public Task<(Response Result, Guid UserId)> CreateUserAsync(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Response> DeleteUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        throw new NotImplementedException();
    }
}
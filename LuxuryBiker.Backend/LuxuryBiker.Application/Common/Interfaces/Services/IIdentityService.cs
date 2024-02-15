namespace LuxuryBiker.Application.Common.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<(Models.Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Models.Result> DeleteUserAsync(string userId);

        Task<bool> AuthorizeAsync(string userId, string policyName);
    }
}
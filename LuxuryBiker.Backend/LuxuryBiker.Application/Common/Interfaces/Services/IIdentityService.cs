namespace LuxuryBiker.Application.Common.Interfaces.Services
{
    public interface IIdentityService<TUser>
    {
        Task<string?> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<TUser> GetUserAsync(string userName);
        Task<TUser> GetUserByIdAsync(string id);

        Task<bool> ValidatePassword(TUser user, string password);

        Task<(Models.Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Models.Result> DeleteUserAsync(string userId);

        Task<List<string>> GetRolesAsync(TUser user);

        Task<bool> AuthorizeAsync(string userId, string policyName);
    }
}
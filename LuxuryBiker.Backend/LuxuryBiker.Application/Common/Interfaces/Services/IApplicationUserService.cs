namespace LuxuryBiker.Application.Common.Interfaces.Services
{
    public interface IApplicationUserService<TUser>
    {
        Task<TUser> GetUserAsync(string userName);
        Task<TUser> GetUserByIdAsync(string id);
        Task<List<string>> GetRolesAsync(TUser user);
        Task<bool> ValidatePassword(TUser user, string password);
    }
}

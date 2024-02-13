namespace LuxuryBiker.Application.Common.Interfaces.Services
{
    public interface IAuthenticationService<TUser> where TUser : class
    {
        Task<ErrorOr<TUser>> Authenticate(string username, string password, bool rememberMe);
        Task<ErrorOr<TUser>> GetCurrentUserProfile();
    }
}
using LuxuryBiker.Domain.Entities.Users;
using LuxuryBiker.Domain.Repositories.Common;

namespace LuxuryBiker.Domain.Repositories.Users
{
    public interface IRepositoryUser : IRepositoryBase<User, int>
    {
        Task<string> GetPasswordAsync(string email);
        Task<User> GetUserAsync(string email);
    }
}
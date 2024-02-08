using LuxuryBiker.Data.Entities.Users;
using LuxuryBiker.Domain.Repositories.Common;

namespace LuxuryBiker.Domain.Repositories.Users
{
    public interface IRepositoryUser : IRepositoryBase<User, int>
    {
    }
}
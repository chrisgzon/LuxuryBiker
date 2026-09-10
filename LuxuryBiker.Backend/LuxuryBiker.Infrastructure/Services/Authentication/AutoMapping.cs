using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Domain.Entities.Users;

namespace LuxuryBiker.Infrastructure.Services.Authentication
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ApplicationUser, AuthenticatedUserDto>().ReverseMap();
        }
    }
}

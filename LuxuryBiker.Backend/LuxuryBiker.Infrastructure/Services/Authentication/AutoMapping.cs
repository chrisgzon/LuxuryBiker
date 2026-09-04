using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Infrastructure.Identity;

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

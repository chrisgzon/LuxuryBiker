using LuxuryBiker.Infrastructure.Services.Authentication;
using LuxuryBiker.Infrastructure.Services.Identity;

namespace LuxuryBiker.Api.Authentication
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
        }
    }
}

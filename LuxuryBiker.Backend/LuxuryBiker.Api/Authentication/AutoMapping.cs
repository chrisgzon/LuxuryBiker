using LuxuryBiker.Domain.Entities.Users;
using LuxuryBiker.Infrastructure.Services.Authentication;

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

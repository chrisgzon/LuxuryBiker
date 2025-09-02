using LuxuryBiker.Application.Thirds.Commands.CreateThird;
using LuxuryBiker.Domain.Entities.Thirds;

namespace LuxuryBiker.Application.Thirds
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ThirdDto, Third>().ReverseMap();
            CreateMap<ThirdTypeDto, TypeThird>().ReverseMap();
        }
    }
}

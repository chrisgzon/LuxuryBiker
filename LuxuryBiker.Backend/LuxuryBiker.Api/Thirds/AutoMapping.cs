using LuxuryBiker.Application.Thirds.Commands.CreateThird;

namespace LuxuryBiker.Api.Thirds
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ThirdModel, ThirdDto>().ReverseMap();
            CreateMap<ThirdTypeModel, ThirdTypeDto>().ReverseMap();
        }
    }
}

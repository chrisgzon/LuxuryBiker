using LuxuryBiker.Application.Thirds.Commands.CreateThird;
using LuxuryBiker.Application.Thirds.Commands.UpdateThird;

namespace LuxuryBiker.Api.Thirds
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ThirdModel, ThirdDto>().ReverseMap();
            CreateMap<ThirdTypeModel, ThirdTypeDto>().ReverseMap();
            CreateMap<UpdateThirdModel, UpdateThirdDto>()
                .ForMember(dest => dest.TypeId,
                           opt => opt.MapFrom(src => src.Type != null ? src.Type.Id : 0));
        }
    }
}

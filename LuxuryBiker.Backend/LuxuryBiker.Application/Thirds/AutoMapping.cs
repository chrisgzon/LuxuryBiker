using LuxuryBiker.Application.Thirds.Commands.CreateThird;
using LuxuryBiker.Application.Thirds.Queries.GetThirds;
using LuxuryBiker.Domain.Entities.Thirds;

namespace LuxuryBiker.Application.Thirds
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ThirdDto, Third>().ReverseMap();
            CreateMap<ThirdTypeDto, TypeThird>().ReverseMap();

            CreateMap<Third, ThirdBriefDto>()
                .ForMember(dest => dest.TypeName,
                           opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : null));
        }
    }
}

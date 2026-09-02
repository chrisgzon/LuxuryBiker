using LuxuryBiker.Application.Sales.Queries.GetSales;
using LuxuryBiker.Domain.Entities.Sales;

namespace LuxuryBiker.Application.Sales
{
    internal class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<Sale, SaleBriefDto>()
                .ForMember(dest => dest.ClientName,
                           opt => opt.MapFrom(src => src.Third != null
                               ? (src.Third.Name + " " + src.Third.Surnames).Trim()
                               : null))
                .ForMember(dest => dest.ProductsQuantity,
                           opt => opt.MapFrom(src => src.Details != null
                               ? src.Details.Sum(d => d.Quantity)
                               : 0m));
        }
    }
}

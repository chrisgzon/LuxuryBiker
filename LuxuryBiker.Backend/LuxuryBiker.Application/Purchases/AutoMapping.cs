using LuxuryBiker.Application.Purchases.Queries.GetPurchases;
using LuxuryBiker.Domain.Entities.Purchases;

namespace LuxuryBiker.Application.Purchases
{
    internal class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<Purchase, PurchaseBriefDto>()
                .ForMember(dest => dest.Date,
                           opt => opt.MapFrom(src => src.DatePurchase))
                .ForMember(dest => dest.SupplierName,
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

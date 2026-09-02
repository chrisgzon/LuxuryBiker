using LuxuryBiker.Application.Purchases.Commands.CreatePurchase;

namespace LuxuryBiker.Api.Purchases
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<PurchaseModel, CreatePurchaseDto>().ReverseMap();
            CreateMap<PurchaseDetailModel, CreatePurchaseDetailDto>().ReverseMap();
        }
    }
}

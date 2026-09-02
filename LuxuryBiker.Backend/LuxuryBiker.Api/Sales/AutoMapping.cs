using LuxuryBiker.Application.Sales.Commands.CreateSale;

namespace LuxuryBiker.Api.Sales
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<SaleModel, CreateSaleDto>().ReverseMap();
            CreateMap<SaleDetailModel, CreateSaleDetailDto>().ReverseMap();
        }
    }
}

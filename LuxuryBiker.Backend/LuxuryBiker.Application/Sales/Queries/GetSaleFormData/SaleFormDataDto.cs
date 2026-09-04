using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Application.Thirds.Queries.GetThirds;

namespace LuxuryBiker.Application.Sales.Queries.GetSaleFormData
{
    public class SaleFormDataDto
    {
        public IReadOnlyList<ProductBriefDto> Products { get; set; } = Array.Empty<ProductBriefDto>();
        public IReadOnlyList<ThirdBriefDto> Clients { get; set; } = Array.Empty<ThirdBriefDto>();
    }
}

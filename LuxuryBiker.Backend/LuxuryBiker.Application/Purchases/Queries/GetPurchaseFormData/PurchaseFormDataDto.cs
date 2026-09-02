using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Application.Thirds.Queries.GetThirds;

namespace LuxuryBiker.Application.Purchases.Queries.GetPurchaseFormData
{
    public class PurchaseFormDataDto
    {
        public IReadOnlyList<ProductBriefDto> Products { get; set; } = Array.Empty<ProductBriefDto>();
        public IReadOnlyList<ThirdBriefDto> Suppliers { get; set; } = Array.Empty<ThirdBriefDto>();
    }
}

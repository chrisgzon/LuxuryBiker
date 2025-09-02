using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;

namespace LuxuryBiker.Domain.Entities.Products
{
    public class Product : BaseAuditableEntity<int>
    {
        public string? Name { get; private set; }
        public string? Code { get; private set; }
        public string? Reference { get; private set; }
        public string? Description { get; private set; }
        public bool? Status { get; private set; }
        public decimal Stock { get; private set; }
        public decimal Value { get; private set; }

        public IEnumerable<PurchaseDetail>? Purchases { get; private set; }
        public IEnumerable<SaleDetail>? Sales { get; private set; }
    }
}

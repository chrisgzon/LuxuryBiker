using LuxuryBiker.Domain.Entities.Common;

namespace LuxuryBiker.Domain.Entities.Purchases
{
    public class PurchaseDetail : BaseEntity<int>
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }
    }
}

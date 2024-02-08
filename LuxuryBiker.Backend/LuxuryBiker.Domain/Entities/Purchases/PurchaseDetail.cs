using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Data.Entities.Compras
{
    public class PurchaseDetail : BaseEntity<int>
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }
    }
}

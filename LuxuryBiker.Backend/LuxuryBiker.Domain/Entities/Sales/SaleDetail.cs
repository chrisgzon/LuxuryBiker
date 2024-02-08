using LuxuryBiker.Domain.Entities.Common;

namespace LuxuryBiker.Domain.Entities.Sales
{
    public class SaleDetail : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public int SaleId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }
    }
}
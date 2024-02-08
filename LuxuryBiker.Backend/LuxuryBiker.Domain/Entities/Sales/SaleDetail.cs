using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Data.Entities.Ventas
{
    public class SaleDetail : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public int SaleId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }

        public Sale Sale { get; set; }
    }
}
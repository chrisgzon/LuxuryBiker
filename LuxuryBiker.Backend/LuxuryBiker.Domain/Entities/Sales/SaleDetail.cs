using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Products;

namespace LuxuryBiker.Domain.Entities.Sales
{
    public class SaleDetail : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public int SaleId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }

        public Sale? Sale { get; set; }
        public Product? Product { get; set; }
    }
}
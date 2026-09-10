using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Products;

namespace LuxuryBiker.Domain.Entities.Sales
{
    public class SaleDetail : BaseEntity<int>
    {
        /// <summary>Constructor para EF Core.</summary>
        private SaleDetail() { }

        public SaleDetail(int productId, decimal productValue, decimal quantity)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId), "El producto es obligatorio.");
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor que cero.");
            if (productValue < 0)
                throw new ArgumentOutOfRangeException(nameof(productValue), "El valor no puede ser negativo.");

            ProductId = productId;
            ProductValue = productValue;
            Quantity = quantity;
        }

        public int ProductId { get; private set; }
        public int SaleId { get; private set; }
        public decimal ProductValue { get; private set; }
        public decimal Quantity { get; private set; }

        public Sale? Sale { get; private set; }
        public Product? Product { get; private set; }

        public decimal Subtotal => Quantity * ProductValue;
    }
}

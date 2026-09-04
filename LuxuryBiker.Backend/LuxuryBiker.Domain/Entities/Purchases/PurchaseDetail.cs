using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Products;

namespace LuxuryBiker.Domain.Entities.Purchases
{
    public class PurchaseDetail : BaseEntity<int>
    {
        /// <summary>Constructor para EF Core.</summary>
        private PurchaseDetail() { }

        public PurchaseDetail(int productId, decimal productValue, decimal quantity)
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

        public int PurchaseId { get; private set; }
        public int ProductId { get; private set; }
        public decimal ProductValue { get; private set; }
        public decimal Quantity { get; private set; }

        public Product? Product { get; private set; }
        public Purchase? Purchase { get; private set; }

        public decimal Subtotal => Quantity * ProductValue;
    }
}

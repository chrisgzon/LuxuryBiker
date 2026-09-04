using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Entities.Users;

namespace LuxuryBiker.Domain.Entities.Purchases
{
    public class Purchase : BaseAuditableEntity<int>
    {
        public Purchase() { }

        public Purchase(string? userId, int? thirdId, DateTimeOffset datePurchase, string code)
        {
            UserId = userId;
            ThirdId = thirdId;
            DatePurchase = datePurchase;
            Code = code;
            Status = true;
            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public string? Code { get; set; }
        public DateTimeOffset DatePurchase { get; set; }
        public string? UserId { get; set; } // user than register the purchase
        public int? ThirdId { get; set; } // supplier than sells to the company
        public decimal Total { get; set; }
        public bool? Status { get; set; }

        public ApplicationUser? User { get; set; }
        public Third? Third { get; set; }
        public IEnumerable<PurchaseDetail>? Details { get; set; }

        /// <summary>
        /// Asigna las líneas de la compra y recalcula el total: suma de
        /// <c>Cantidad * Valor</c> por línea, más el IVA si aplica.
        /// </summary>
        public void SetDetails(IEnumerable<PurchaseDetail> details, bool applyIva, decimal ivaRate)
        {
            var lines = details.ToList();
            Details = lines;

            decimal subtotal = lines.Sum(line => line.Quantity * line.ProductValue);
            decimal iva = applyIva ? subtotal * (ivaRate / 100m) : 0m;

            Total = subtotal + iva;
        }

        /// <summary>Invierte el estado (validada &lt;-&gt; cancelada).</summary>
        public void ToggleStatus()
        {
            Status = !(Status ?? true);
            LastModified = DateTime.Now;
        }
    }
}

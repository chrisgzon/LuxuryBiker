using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Entities.Users;

namespace LuxuryBiker.Domain.Entities.Sales
{
    public class Sale : BaseAuditableEntity<int>
    {
        public Sale() { }

        public Sale(string? userId, int? thirdId, DateTimeOffset date, string code)
        {
            UserId = userId;
            ThirdId = thirdId;
            Date = date;
            Code = code;
            Status = true;
            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public string? Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public int? ThirdId { get; set; } // client who buys from the company
        public string? UserId { get; set; } // user that registers the sale
        public bool? Status { get; set; }
        public decimal Total { get; set; }

        public ApplicationUser? User { get; set; }
        public Third? Third { get; set; }
        public IEnumerable<SaleDetail>? Details { get; set; }

        /// <summary>
        /// Asigna las líneas de la venta y recalcula el total: suma de
        /// <c>Cantidad * Valor</c> por línea, más el IVA si aplica.
        /// </summary>
        public void SetDetails(IEnumerable<SaleDetail> details, bool applyIva, decimal ivaRate)
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

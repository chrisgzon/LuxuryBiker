using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Thirds;

namespace LuxuryBiker.Domain.Entities.Purchases
{
    /// <summary>
    /// Raíz del agregado Compra. Las líneas solo se pueden añadir a través de la fábrica,
    /// de modo que <see cref="Total"/> siempre es coherente con el detalle.
    /// </summary>
    public class Purchase : BaseAuditableEntity<int>
    {
        private readonly List<PurchaseDetail> _details = new();

        /// <summary>Constructor para EF Core.</summary>
        private Purchase() { }

        private Purchase(string? userId, int? thirdId, DateTimeOffset datePurchase, string code)
        {
            UserId = userId;
            ThirdId = thirdId;
            DatePurchase = datePurchase;
            Code = code;
            Status = true;
            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public static Purchase Register(
            string? userId,
            int? thirdId,
            DateTimeOffset datePurchase,
            string code,
            IEnumerable<PurchaseDetail> details,
            bool applyIva,
            decimal ivaRate)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El código de la compra es obligatorio.", nameof(code));

            var lines = details?.ToList() ?? throw new ArgumentNullException(nameof(details));
            if (lines.Count == 0)
                throw new ArgumentException("La compra debe incluir al menos una línea.", nameof(details));

            var purchase = new Purchase(userId, thirdId, datePurchase, code);
            purchase._details.AddRange(lines);
            purchase.RecalculateTotal(applyIva, ivaRate);
            return purchase;
        }

        public string? Code { get; private set; }
        public DateTimeOffset DatePurchase { get; private set; }
        public string? UserId { get; private set; }   // usuario que registra la compra
        public int? ThirdId { get; private set; }     // proveedor que vende a la empresa
        public decimal Total { get; private set; }
        public bool? Status { get; private set; }

        public Third? Third { get; private set; }
        public IReadOnlyCollection<PurchaseDetail> Details => _details;

        /// <summary>Invierte el estado (validada &lt;-&gt; cancelada).</summary>
        public void ToggleStatus()
        {
            Status = !(Status ?? true);
            LastModified = DateTime.Now;
        }

        /// <summary>Suma de las líneas más el IVA cuando aplica.</summary>
        private void RecalculateTotal(bool applyIva, decimal ivaRate)
        {
            decimal subtotal = _details.Sum(line => line.Subtotal);
            decimal iva = applyIva ? subtotal * (ivaRate / 100m) : 0m;

            Total = subtotal + iva;
        }
    }
}

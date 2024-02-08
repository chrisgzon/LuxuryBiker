using LuxuryBiker.Domain.Entities.Common;

namespace LuxuryBiker.Domain.Entities.Sales
{
    public class Sale : BaseAuditableEntity<int>
    {
        public string? Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public int? ThirdId { get; set; } // third than makes the purchase
        public Guid? UserId { get; set; } // user than register the sale
        public bool Estado { get; set; }
        public decimal Total { get; set; }

    }
}

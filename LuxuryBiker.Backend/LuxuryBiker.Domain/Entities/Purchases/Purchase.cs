using LuxuryBiker.Domain.Entities.Common;

namespace LuxuryBiker.Domain.Entities.Purchases
{
    public class Purchase : BaseAuditableEntity<int>
    {
        public string? Code { get; set; }
        public DateTimeOffset DatePurchase { get; set; }
        public Guid? UserId { get; set; } // user than register the purchase
        public int ThirdId { get; set; } // third than makes the sale
        public decimal Total { get; set; }
        public bool State { get; set; }
    }
}

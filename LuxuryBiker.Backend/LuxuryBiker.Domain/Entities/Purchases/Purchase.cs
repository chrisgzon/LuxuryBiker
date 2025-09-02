using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Entities.Users;

namespace LuxuryBiker.Domain.Entities.Purchases
{
    public class Purchase : BaseAuditableEntity<int>
    {
        public string? Code { get; set; }
        public DateTimeOffset DatePurchase { get; set; }
        public string? UserId { get; set; } // user than register the purchase
        public int? ThirdId { get; set; } // third than makes the sale
        public decimal Total { get; set; }
        public bool? Status { get; set; }

        public ApplicationUser? User { get; set; }
        public Third? Third { get; set; }
        public IEnumerable<PurchaseDetail>? Details { get; set; }
    }
}

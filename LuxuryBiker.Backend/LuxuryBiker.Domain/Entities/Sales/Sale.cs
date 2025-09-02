using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Entities.Users;

namespace LuxuryBiker.Domain.Entities.Sales
{
    public class Sale : BaseAuditableEntity<int>
    {
        public string? Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public int? ThirdId { get; set; } // third than makes the purchase
        public string? UserId { get; set; } // user than register the sale
        public bool? Status { get; set; }
        public decimal Total { get; set; }

        public ApplicationUser? User { get; set; }
        public Third? Third { get; set; }
        public IEnumerable<SaleDetail>? Details { get; set; }
    }
}

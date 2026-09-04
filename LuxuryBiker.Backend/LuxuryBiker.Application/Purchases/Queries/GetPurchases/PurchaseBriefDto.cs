namespace LuxuryBiker.Application.Purchases.Queries.GetPurchases
{
    public class PurchaseBriefDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Total { get; set; }
        public bool? Status { get; set; }
        public string? SupplierName { get; set; }
        public decimal ProductsQuantity { get; set; }
    }
}

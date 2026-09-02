namespace LuxuryBiker.Api.Purchases
{
    public class PurchaseModel
    {
        public int? ThirdId { get; set; }
        public DateTimeOffset DatePurchase { get; set; }
        public bool ApplyIva { get; set; }
        public List<PurchaseDetailModel> Details { get; set; } = new();
    }

    public class PurchaseDetailModel
    {
        public int ProductId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }
    }
}

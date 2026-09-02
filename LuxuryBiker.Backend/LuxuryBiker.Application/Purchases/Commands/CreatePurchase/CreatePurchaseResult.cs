namespace LuxuryBiker.Application.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseResult
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}

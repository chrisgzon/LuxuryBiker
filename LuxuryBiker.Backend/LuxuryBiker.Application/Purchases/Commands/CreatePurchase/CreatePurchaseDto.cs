namespace LuxuryBiker.Application.Purchases.Commands.CreatePurchase
{
    public class CreatePurchaseDto
    {
        public int? ThirdId { get; set; }
        public DateTimeOffset DatePurchase { get; set; }
        public bool ApplyIva { get; set; }
        public List<CreatePurchaseDetailDto> Details { get; set; } = new();
    }

    public class CreatePurchaseDetailDto
    {
        public int ProductId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }
    }
}

namespace LuxuryBiker.Api.Sales
{
    public class SaleModel
    {
        public int? ThirdId { get; set; }
        public bool ApplyIva { get; set; }
        public List<SaleDetailModel> Details { get; set; } = new();
    }

    public class SaleDetailModel
    {
        public int ProductId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }
    }
}

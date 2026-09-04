namespace LuxuryBiker.Application.Sales.Commands.CreateSale
{
    public class CreateSaleDto
    {
        public int? ThirdId { get; set; }
        public bool ApplyIva { get; set; }
        public List<CreateSaleDetailDto> Details { get; set; } = new();
    }

    public class CreateSaleDetailDto
    {
        public int ProductId { get; set; }
        public decimal ProductValue { get; set; }
        public decimal Quantity { get; set; }
    }
}

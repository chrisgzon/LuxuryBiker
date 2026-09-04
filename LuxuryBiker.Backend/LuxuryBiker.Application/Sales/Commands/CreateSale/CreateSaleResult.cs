namespace LuxuryBiker.Application.Sales.Commands.CreateSale
{
    public class CreateSaleResult
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}

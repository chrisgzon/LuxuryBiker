namespace LuxuryBiker.Application.Sales.Queries.GetSales
{
    public class SaleBriefDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Total { get; set; }
        public bool? Status { get; set; }
        public string? ClientName { get; set; }
        public decimal ProductsQuantity { get; set; }
    }
}

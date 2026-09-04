namespace LuxuryBiker.Application.Products.Queries.GetProducts
{
    public class ProductBriefDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool? Status { get; set; }
        public decimal? Stock { get; set; }
        public decimal? Value { get; set; }
    }
}

namespace LuxuryBiker.Api.Products
{
    public class ProductModel
    {
        public string Name { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? Description { get; set; }
    }
}

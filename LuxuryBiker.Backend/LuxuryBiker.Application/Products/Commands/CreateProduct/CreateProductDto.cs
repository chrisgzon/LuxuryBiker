namespace LuxuryBiker.Application.Products.Commands.CreateProduct
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? Description { get; set; }
    }
}

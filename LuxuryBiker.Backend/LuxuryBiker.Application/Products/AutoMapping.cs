using LuxuryBiker.Application.Products.Commands.CreateProduct;
using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Domain.Entities.Products;

namespace LuxuryBiker.Application.Products
{
    internal class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateProductDto, Product>()
            .ConstructUsing(src => new Product(src.Name, null, src.Reference, src.Description, src.Status, null, null));

            CreateMap<Product, ProductBriefDto>();
        }
    }
}

using LuxuryBiker.Application.Products.Commands.CreateProduct;
using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Domain.Entities.Products;

namespace LuxuryBiker.Application.Products
{
    internal class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            // El alta de producto se construye con `Product.Create`, no por mapeo:
            // así el agregado nunca existe sin código interno.
            CreateMap<Product, ProductBriefDto>();
        }
    }
}

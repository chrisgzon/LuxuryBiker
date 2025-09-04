using LuxuryBiker.Application.Products.Commands.CreateProduct;

namespace LuxuryBiker.Api.Products
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ProductModel, CreateProductDto>().ReverseMap();
        }
    }
}

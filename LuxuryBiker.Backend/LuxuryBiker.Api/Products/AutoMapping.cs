using LuxuryBiker.Application.Products.Commands.CreateProduct;
using LuxuryBiker.Application.Products.Commands.UpdateProduct;

namespace LuxuryBiker.Api.Products
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<ProductModel, CreateProductDto>().ReverseMap();
            CreateMap<UpdateProductModel, UpdateProductDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Products;

namespace LuxuryBiker.Application.Products.Queries.GetProductById
{
    [Authorize]
    public record GetProductByIdQuery(int Id) : IRequest<ErrorOr<ProductBriefDto>>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductBriefDto>>
    {
        private readonly IProductsRepository _repository;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<ProductBriefDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _repository.GetAsync(request.Id);
            if (product is null)
                return ProductsErrors.NotFound;

            return _mapper.Map<ProductBriefDto>(product);
        }
    }
}

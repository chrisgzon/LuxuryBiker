using AutoMapper;
using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Repositories.Products;

namespace LuxuryBiker.Application.Products.Queries.GetProducts
{
    [Authorize]
    public record GetProductsQuery(int PageNumber = 1, int PageSize = 20, bool OnlyActive = true)
        : IRequest<ErrorOr<PaginatedList<ProductBriefDto>>>;

    public class GetProductsQueryHandler
        : IRequestHandler<GetProductsQuery, ErrorOr<PaginatedList<ProductBriefDto>>>
    {
        private readonly IProductsRepository _repository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PaginatedList<ProductBriefDto>>> Handle(
            GetProductsQuery request, CancellationToken cancellationToken)
        {
            (IReadOnlyList<Product> items, int totalCount) = await _repository.GetPagedAsync(
                request.PageNumber, request.PageSize, request.OnlyActive, cancellationToken);

            var dtos = _mapper.Map<IReadOnlyList<ProductBriefDto>>(items);

            return new PaginatedList<ProductBriefDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}

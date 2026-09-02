using AutoMapper;
using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Application.Thirds.Queries.GetThirds;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Thirds;

namespace LuxuryBiker.Application.Sales.Queries.GetSaleFormData
{
    [Authorize]
    public record GetSaleFormDataQuery : IRequest<ErrorOr<SaleFormDataDto>>;

    public class GetSaleFormDataQueryHandler
        : IRequestHandler<GetSaleFormDataQuery, ErrorOr<SaleFormDataDto>>
    {
        private const int MaxComboItems = 1000;

        private readonly IProductsRepository _productsRepository;
        private readonly IThirdRepository _thirdRepository;
        private readonly IMapper _mapper;

        public GetSaleFormDataQueryHandler(
            IProductsRepository productsRepository,
            IThirdRepository thirdRepository,
            IMapper mapper)
        {
            _productsRepository = productsRepository;
            _thirdRepository = thirdRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<SaleFormDataDto>> Handle(
            GetSaleFormDataQuery request, CancellationToken cancellationToken)
        {
            var (products, _) = await _productsRepository.GetPagedAsync(
                1, MaxComboItems, onlyActive: true, cancellationToken);

            var (clients, _) = await _thirdRepository.GetPagedAsync(
                1, MaxComboItems, ThirdTypes.Client, cancellationToken);

            return new SaleFormDataDto
            {
                Products = _mapper.Map<IReadOnlyList<ProductBriefDto>>(products),
                Clients = _mapper.Map<IReadOnlyList<ThirdBriefDto>>(clients)
            };
        }
    }
}

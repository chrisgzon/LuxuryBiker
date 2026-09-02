using AutoMapper;
using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Application.Thirds.Queries.GetThirds;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Thirds;

namespace LuxuryBiker.Application.Purchases.Queries.GetPurchaseFormData
{
    [Authorize]
    public record GetPurchaseFormDataQuery : IRequest<ErrorOr<PurchaseFormDataDto>>;

    public class GetPurchaseFormDataQueryHandler
        : IRequestHandler<GetPurchaseFormDataQuery, ErrorOr<PurchaseFormDataDto>>
    {
        // Suficiente para poblar los combos del formulario de compra sin paginar.
        private const int MaxComboItems = 1000;

        private readonly IProductsRepository _productsRepository;
        private readonly IThirdRepository _thirdRepository;
        private readonly IMapper _mapper;

        public GetPurchaseFormDataQueryHandler(
            IProductsRepository productsRepository,
            IThirdRepository thirdRepository,
            IMapper mapper)
        {
            _productsRepository = productsRepository;
            _thirdRepository = thirdRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PurchaseFormDataDto>> Handle(
            GetPurchaseFormDataQuery request, CancellationToken cancellationToken)
        {
            var (products, _) = await _productsRepository.GetPagedAsync(
                1, MaxComboItems, onlyActive: true, cancellationToken);

            var (suppliers, _) = await _thirdRepository.GetPagedAsync(
                1, MaxComboItems, ThirdTypes.Provider, cancellationToken);

            return new PurchaseFormDataDto
            {
                Products = _mapper.Map<IReadOnlyList<ProductBriefDto>>(products),
                Suppliers = _mapper.Map<IReadOnlyList<ThirdBriefDto>>(suppliers)
            };
        }
    }
}

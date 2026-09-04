using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Domain.Common;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Purchases;
using LuxuryBiker.Domain.Repositories.Thirds;

namespace LuxuryBiker.Application.Purchases.Commands.CreatePurchase
{
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Seller}")]
    public record CreatePurchaseCommand(CreatePurchaseDto Dto) : IRequest<ErrorOr<CreatePurchaseResult>>;

    public class CreatePurchaseCommandHandler
        : IRequestHandler<CreatePurchaseCommand, ErrorOr<CreatePurchaseResult>>
    {
        private readonly IPurchasesRepository _purchasesRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IThirdRepository _thirdRepository;
        private readonly IUser _user;

        public CreatePurchaseCommandHandler(
            IPurchasesRepository purchasesRepository,
            IProductsRepository productsRepository,
            IThirdRepository thirdRepository,
            IUser user)
        {
            _purchasesRepository = purchasesRepository;
            _productsRepository = productsRepository;
            _thirdRepository = thirdRepository;
            _user = user;
        }

        public async Task<ErrorOr<CreatePurchaseResult>> Handle(
            CreatePurchaseCommand request, CancellationToken cancellationToken)
        {
            CreatePurchaseDto dto = request.Dto;

            if (dto.Details.Count == 0)
                return PurchasesErrors.EmptyDetails;

            if (dto.ThirdId.HasValue)
            {
                var supplier = await _thirdRepository.GetAsync(dto.ThirdId.Value);
                if (supplier is null)
                    return PurchasesErrors.SupplierNotFound;
            }

            var productIds = dto.Details.Select(d => d.ProductId).Distinct().ToList();
            IReadOnlyList<Product> products = await _productsRepository.GetByIdsAsync(productIds, cancellationToken);

            var productsById = products.ToDictionary(p => p.Id);
            if (products.Count != productIds.Count)
            {
                int missingProductId = productIds.First(id => !productsById.ContainsKey(id));
                return PurchasesErrors.ProductNotFound(missingProductId);
            }

            string code = DocumentNumber.Next(
                DocumentNumber.PurchasePrefix, await _purchasesRepository.GetLastCodeAsync(cancellationToken));

            var details = dto.Details.Select(d => new PurchaseDetail(d.ProductId, d.ProductValue, d.Quantity));

            var purchase = Purchase.Register(
                _user.Id, dto.ThirdId, dto.DatePurchase, code, details, dto.ApplyIva, Taxes.IvaRate);
            purchase.CreatedBy = _user.Id;
            purchase.LastModifiedBy = _user.Id;

            // Ajuste de inventario: la compra aumenta el stock y actualiza el último valor
            // de compra de cada producto (comportamiento del sistema legado).
            foreach (var line in dto.Details)
            {
                Product product = productsById[line.ProductId];
                product.IncreaseStock(line.Quantity);
                product.RegisterPurchaseValue(line.ProductValue);
            }

            await _purchasesRepository.CreateAsync(purchase, cancellationToken);

            return new CreatePurchaseResult
            {
                Id = purchase.Id,
                Code = purchase.Code!,
                Total = purchase.Total
            };
        }
    }
}

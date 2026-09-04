using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Purchases;

namespace LuxuryBiker.Application.Purchases.Commands.ChangePurchaseStatus
{
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Seller}")]
    public record ChangePurchaseStatusCommand(int PurchaseId) : IRequest<ErrorOr<ChangeStatusResult>>;

    public class ChangePurchaseStatusCommandHandler
        : IRequestHandler<ChangePurchaseStatusCommand, ErrorOr<ChangeStatusResult>>
    {
        private readonly IPurchasesRepository _purchasesRepository;
        private readonly IProductsRepository _productsRepository;

        public ChangePurchaseStatusCommandHandler(
            IPurchasesRepository purchasesRepository,
            IProductsRepository productsRepository)
        {
            _purchasesRepository = purchasesRepository;
            _productsRepository = productsRepository;
        }

        public async Task<ErrorOr<ChangeStatusResult>> Handle(
            ChangePurchaseStatusCommand request, CancellationToken cancellationToken)
        {
            Purchase? purchase = await _purchasesRepository.GetByIdWithDetailsAsync(
                request.PurchaseId, cancellationToken);

            if (purchase is null)
                return PurchasesErrors.NotFound;

            purchase.ToggleStatus();
            bool nowValidated = purchase.Status == true;

            var lines = (purchase.Details ?? Enumerable.Empty<PurchaseDetail>()).ToList();
            var productIds = lines.Select(l => l.ProductId).Distinct().ToList();
            IReadOnlyList<Product> products = await _productsRepository.GetForUpdateAsync(productIds, cancellationToken);
            var productsById = products.ToDictionary(p => p.Id);

            // Una compra validada suma al stock; cancelarla lo revierte
            // (equivalente al trigger tr_updStockCompraChangeStatus del legado).
            foreach (var line in lines)
            {
                if (!productsById.TryGetValue(line.ProductId, out Product? product))
                    continue;

                if (nowValidated)
                    product.IncreaseStock(line.Quantity);
                else
                    product.RevertPurchase(line.Quantity);
            }

            await _purchasesRepository.SaveChangesAsync(cancellationToken);

            return new ChangeStatusResult { Id = purchase.Id, Status = nowValidated };
        }
    }
}

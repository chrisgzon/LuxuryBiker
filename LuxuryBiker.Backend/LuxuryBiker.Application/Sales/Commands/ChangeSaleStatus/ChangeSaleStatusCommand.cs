using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Sales;

namespace LuxuryBiker.Application.Sales.Commands.ChangeSaleStatus
{
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Seller}")]
    public record ChangeSaleStatusCommand(int SaleId) : IRequest<ErrorOr<ChangeStatusResult>>;

    public class ChangeSaleStatusCommandHandler
        : IRequestHandler<ChangeSaleStatusCommand, ErrorOr<ChangeStatusResult>>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IProductsRepository _productsRepository;

        public ChangeSaleStatusCommandHandler(
            ISalesRepository salesRepository,
            IProductsRepository productsRepository)
        {
            _salesRepository = salesRepository;
            _productsRepository = productsRepository;
        }

        public async Task<ErrorOr<ChangeStatusResult>> Handle(
            ChangeSaleStatusCommand request, CancellationToken cancellationToken)
        {
            Sale? sale = await _salesRepository.GetByIdWithDetailsAsync(request.SaleId, cancellationToken);

            if (sale is null)
                return SalesErrors.NotFound;

            sale.ToggleStatus();
            bool nowValidated = sale.Status == true;

            var lines = (sale.Details ?? Enumerable.Empty<SaleDetail>()).ToList();
            var productIds = lines.Select(l => l.ProductId).Distinct().ToList();
            IReadOnlyList<Product> products = await _productsRepository.GetForUpdateAsync(productIds, cancellationToken);
            var productsById = products.ToDictionary(p => p.Id);

            // Una venta validada descuenta del stock; cancelarla lo reintegra
            // (equivalente al trigger tr_updStockVentaChangeStatus del legado).
            foreach (var line in lines)
            {
                if (!productsById.TryGetValue(line.ProductId, out Product? product))
                    continue;

                if (nowValidated)
                    product.DecreaseStock(line.Quantity);
                else
                    product.IncreaseStock(line.Quantity);
            }

            await _salesRepository.SaveChangesAsync(cancellationToken);

            return new ChangeStatusResult { Id = sale.Id, Status = nowValidated };
        }
    }
}

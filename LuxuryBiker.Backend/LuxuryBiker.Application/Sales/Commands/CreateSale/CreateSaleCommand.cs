using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Domain.Common;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Sales;
using LuxuryBiker.Domain.Repositories.Thirds;

namespace LuxuryBiker.Application.Sales.Commands.CreateSale
{
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Seller}")]
    public record CreateSaleCommand(CreateSaleDto Dto) : IRequest<ErrorOr<CreateSaleResult>>;

    public class CreateSaleCommandHandler
        : IRequestHandler<CreateSaleCommand, ErrorOr<CreateSaleResult>>
    {
        private readonly ISalesRepository _salesRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IThirdRepository _thirdRepository;
        private readonly IUser _user;

        public CreateSaleCommandHandler(
            ISalesRepository salesRepository,
            IProductsRepository productsRepository,
            IThirdRepository thirdRepository,
            IUser user)
        {
            _salesRepository = salesRepository;
            _productsRepository = productsRepository;
            _thirdRepository = thirdRepository;
            _user = user;
        }

        public async Task<ErrorOr<CreateSaleResult>> Handle(
            CreateSaleCommand request, CancellationToken cancellationToken)
        {
            CreateSaleDto dto = request.Dto;

            if (dto.Details.Count == 0)
                return SalesErrors.EmptyDetails;

            if (dto.ThirdId.HasValue)
            {
                var client = await _thirdRepository.GetAsync(dto.ThirdId.Value);
                if (client is null)
                    return SalesErrors.ClientNotFound;
            }

            var productIds = dto.Details.Select(d => d.ProductId).Distinct().ToList();
            IReadOnlyList<Product> products = await _productsRepository.GetForUpdateAsync(productIds, cancellationToken);
            var productsById = products.ToDictionary(p => p.Id);

            if (products.Count != productIds.Count)
            {
                int missingProductId = productIds.First(id => !productsById.ContainsKey(id));
                return SalesErrors.ProductNotFound(missingProductId);
            }

            // Se valida existencia de inventario antes de descontar (a diferencia del legado,
            // que permitía stock negativo).
            foreach (var group in dto.Details.GroupBy(d => d.ProductId))
            {
                decimal requested = group.Sum(d => d.Quantity);
                Product product = productsById[group.Key];
                if (!product.HasStockFor(requested))
                    return SalesErrors.InsufficientStock(product.Name, product.Stock ?? 0m, requested);
            }

            string code = DocumentNumber.Next(
                DocumentNumber.SalePrefix, await _salesRepository.GetLastCodeAsync(cancellationToken));

            var details = dto.Details.Select(d => new SaleDetail(d.ProductId, d.ProductValue, d.Quantity));

            var sale = Sale.Register(
                _user.Id, dto.ThirdId, DateTimeOffset.Now, code, details, dto.ApplyIva, Taxes.IvaRate);
            sale.CreatedBy = _user.Id;
            sale.LastModifiedBy = _user.Id;

            // La venta validada descuenta del inventario (comportamiento del sistema legado).
            foreach (var line in dto.Details)
            {
                productsById[line.ProductId].Sell(line.Quantity);
            }

            await _salesRepository.CreateAsync(sale, cancellationToken);

            return new CreateSaleResult
            {
                Id = sale.Id,
                Code = sale.Code!,
                Total = sale.Total
            };
        }
    }
}

using FluentAssertions;
using LuxuryBiker.Application.Purchases.Commands.ChangePurchaseStatus;
using LuxuryBiker.Application.Sales.Commands.ChangeSaleStatus;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Purchases;
using LuxuryBiker.Domain.Repositories.Sales;
using Moq;
using Xunit;

namespace LuxuryBiker.Application.UnitTests
{
    public class ChangeStatusCommandHandlersTests
    {
        private readonly Mock<IProductsRepository> _productsRepository = new();

        private void HaveProducts(params Product[] products) =>
            _productsRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products.ToList());

        [Fact]
        public async Task Cancelling_a_validated_sale_returns_stock()
        {
            var product = TestData.Product(id: 1, stock: 5m);
            HaveProducts(product);

            var sale = new Sale("user-1", null, DateTimeOffset.Now, "VLB3") { Id = 7 };
            sale.SetDetails(new[] { new SaleDetail { ProductId = 1, ProductValue = 100m, Quantity = 2m } }, false, Taxes.IvaRate);

            var salesRepository = new Mock<ISalesRepository>();
            salesRepository.Setup(r => r.GetByIdWithDetailsAsync(7, It.IsAny<CancellationToken>())).ReturnsAsync(sale);
            salesRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new ChangeSaleStatusCommandHandler(salesRepository.Object, _productsRepository.Object);
            var result = await handler.Handle(new ChangeSaleStatusCommand(7), CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Status.Should().BeFalse(); // validada -> cancelada
            product.Stock.Should().Be(7m);          // 5 + 2 reintegrado
        }

        [Fact]
        public async Task Change_status_on_missing_sale_returns_not_found()
        {
            var salesRepository = new Mock<ISalesRepository>();
            salesRepository.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Sale?)null);

            var handler = new ChangeSaleStatusCommandHandler(salesRepository.Object, _productsRepository.Object);
            var result = await handler.Handle(new ChangeSaleStatusCommand(123), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Sales.NotFound");
        }

        [Fact]
        public async Task Cancelling_a_validated_purchase_removes_stock()
        {
            var product = TestData.Product(id: 1, stock: 5m);
            HaveProducts(product);

            var purchase = new Purchase("user-1", null, DateTimeOffset.Now, "CLB3") { Id = 4 };
            purchase.SetDetails(new[] { new PurchaseDetail { ProductId = 1, ProductValue = 100m, Quantity = 2m } }, false, Taxes.IvaRate);

            var purchasesRepository = new Mock<IPurchasesRepository>();
            purchasesRepository.Setup(r => r.GetByIdWithDetailsAsync(4, It.IsAny<CancellationToken>())).ReturnsAsync(purchase);
            purchasesRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = new ChangePurchaseStatusCommandHandler(purchasesRepository.Object, _productsRepository.Object);
            var result = await handler.Handle(new ChangePurchaseStatusCommand(4), CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Status.Should().BeFalse();
            product.Stock.Should().Be(3m); // 5 - 2 revertido
        }
    }
}

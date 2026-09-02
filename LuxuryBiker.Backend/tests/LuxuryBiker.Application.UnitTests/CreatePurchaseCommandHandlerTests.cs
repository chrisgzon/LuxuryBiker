using FluentAssertions;
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Application.Purchases.Commands.CreatePurchase;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Purchases;
using LuxuryBiker.Domain.Repositories.Thirds;
using Moq;
using Xunit;

namespace LuxuryBiker.Application.UnitTests
{
    public class CreatePurchaseCommandHandlerTests
    {
        private readonly Mock<IPurchasesRepository> _purchasesRepository = new();
        private readonly Mock<IProductsRepository> _productsRepository = new();
        private readonly Mock<IThirdRepository> _thirdRepository = new();
        private readonly Mock<IUser> _user = new();

        private Purchase? _createdPurchase;

        public CreatePurchaseCommandHandlerTests()
        {
            _user.SetupGet(u => u.Id).Returns("user-1");
            _purchasesRepository.Setup(r => r.GetLastCodeAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync("CLB4");
            _purchasesRepository.Setup(r => r.CreateAsync(It.IsAny<Purchase>(), It.IsAny<CancellationToken>()))
                .Callback<Purchase, CancellationToken>((purchase, _) => _createdPurchase = purchase)
                .Returns(Task.CompletedTask);
        }

        private CreatePurchaseCommandHandler CreateHandler() => new(
            _purchasesRepository.Object, _productsRepository.Object, _thirdRepository.Object, _user.Object);

        private void HaveProducts(params Product[] products) =>
            _productsRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products.ToList());

        [Fact]
        public async Task Registers_purchase_applies_iva_and_increases_stock_and_value()
        {
            var product = TestData.Product(id: 1, stock: 5m, value: 0m);
            HaveProducts(product);

            var dto = new CreatePurchaseDto
            {
                ThirdId = null,
                DatePurchase = DateTimeOffset.Now,
                ApplyIva = true,
                Details = { new CreatePurchaseDetailDto { ProductId = 1, ProductValue = 1000m, Quantity = 3m } }
            };

            var result = await CreateHandler().Handle(new CreatePurchaseCommand(dto), CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Code.Should().Be("CLB5");
            result.Value.Total.Should().Be(3570m); // 3000 + 19%
            _createdPurchase!.Total.Should().Be(3570m);
            product.Stock.Should().Be(8m);   // 5 + 3
            product.Value.Should().Be(1000m); // último valor de compra
        }

        [Fact]
        public async Task Total_without_iva_is_the_sum_of_the_lines()
        {
            HaveProducts(TestData.Product(1, 0m), TestData.Product(2, 0m));

            var dto = new CreatePurchaseDto
            {
                DatePurchase = DateTimeOffset.Now,
                ApplyIva = false,
                Details =
                {
                    new CreatePurchaseDetailDto { ProductId = 1, ProductValue = 1000m, Quantity = 2m },
                    new CreatePurchaseDetailDto { ProductId = 2, ProductValue = 500m, Quantity = 1m }
                }
            };

            var result = await CreateHandler().Handle(new CreatePurchaseCommand(dto), CancellationToken.None);

            result.Value.Total.Should().Be(2500m);
        }

        [Fact]
        public async Task Fails_when_there_are_no_details()
        {
            var result = await CreateHandler().Handle(
                new CreatePurchaseCommand(new CreatePurchaseDto()), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Purchases.EmptyDetails");
        }

        [Fact]
        public async Task Fails_when_a_product_does_not_exist()
        {
            HaveProducts(); // vacío

            var dto = new CreatePurchaseDto
            {
                DatePurchase = DateTimeOffset.Now,
                Details = { new CreatePurchaseDetailDto { ProductId = 99, ProductValue = 10m, Quantity = 1m } }
            };

            var result = await CreateHandler().Handle(new CreatePurchaseCommand(dto), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Purchases.ProductNotFound");
        }

        [Fact]
        public async Task Fails_when_the_supplier_does_not_exist()
        {
            HaveProducts(TestData.Product(1, 0m));
            _thirdRepository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((Domain.Entities.Thirds.Third?)null);

            var dto = new CreatePurchaseDto
            {
                ThirdId = 42,
                DatePurchase = DateTimeOffset.Now,
                Details = { new CreatePurchaseDetailDto { ProductId = 1, ProductValue = 10m, Quantity = 1m } }
            };

            var result = await CreateHandler().Handle(new CreatePurchaseCommand(dto), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Purchases.SupplierNotFound");
        }
    }
}

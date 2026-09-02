using FluentAssertions;
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Application.Sales.Commands.CreateSale;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Sales;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Sales;
using LuxuryBiker.Domain.Repositories.Thirds;
using Moq;
using Xunit;

namespace LuxuryBiker.Application.UnitTests
{
    public class CreateSaleCommandHandlerTests
    {
        private readonly Mock<ISalesRepository> _salesRepository = new();
        private readonly Mock<IProductsRepository> _productsRepository = new();
        private readonly Mock<IThirdRepository> _thirdRepository = new();
        private readonly Mock<IUser> _user = new();

        private Sale? _createdSale;

        public CreateSaleCommandHandlerTests()
        {
            _user.SetupGet(u => u.Id).Returns("user-1");
            _salesRepository.Setup(r => r.GetLastCodeAsync(It.IsAny<CancellationToken>())).ReturnsAsync("VLB9");
            _salesRepository.Setup(r => r.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .Callback<Sale, CancellationToken>((sale, _) => _createdSale = sale)
                .Returns(Task.CompletedTask);
        }

        private CreateSaleCommandHandler CreateHandler() => new(
            _salesRepository.Object, _productsRepository.Object, _thirdRepository.Object, _user.Object);

        [Fact]
        public async Task Registers_sale_and_decreases_stock()
        {
            var product = TestData.Product(id: 1, stock: 10m);
            _productsRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product> { product });

            var dto = new CreateSaleDto
            {
                ThirdId = null,
                ApplyIva = false,
                Details = { new CreateSaleDetailDto { ProductId = 1, ProductValue = 2000m, Quantity = 4m } }
            };

            var result = await CreateHandler().Handle(new CreateSaleCommand(dto), CancellationToken.None);

            result.IsError.Should().BeFalse();
            result.Value.Code.Should().Be("VLB10");
            result.Value.Total.Should().Be(8000m);
            _createdSale!.Total.Should().Be(8000m);
            product.Stock.Should().Be(6m); // 10 - 4
        }

        [Fact]
        public async Task Fails_when_there_are_no_details()
        {
            var result = await CreateHandler().Handle(
                new CreateSaleCommand(new CreateSaleDto()), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Sales.EmptyDetails");
        }

        [Fact]
        public async Task Fails_when_stock_is_insufficient()
        {
            var product = TestData.Product(id: 1, stock: 3m);
            _productsRepository.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product> { product });

            var dto = new CreateSaleDto
            {
                Details = { new CreateSaleDetailDto { ProductId = 1, ProductValue = 100m, Quantity = 5m } }
            };

            var result = await CreateHandler().Handle(new CreateSaleCommand(dto), CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Sales.InsufficientStock");
            product.Stock.Should().Be(3m); // no se descontó
        }
    }
}

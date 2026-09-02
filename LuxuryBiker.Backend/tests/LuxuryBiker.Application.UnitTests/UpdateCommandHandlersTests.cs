using FluentAssertions;
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Application.Products.Commands.UpdateProduct;
using LuxuryBiker.Application.Thirds.Commands.UpdateThird;
using LuxuryBiker.Domain.Entities.Products;
using LuxuryBiker.Domain.Entities.Thirds;
using LuxuryBiker.Domain.Repositories.Products;
using LuxuryBiker.Domain.Repositories.Thirds;
using Moq;
using Xunit;

namespace LuxuryBiker.Application.UnitTests
{
    public class UpdateCommandHandlersTests
    {
        private readonly Mock<IUser> _user = new();

        public UpdateCommandHandlersTests() => _user.SetupGet(u => u.Id).Returns("user-1");

        [Fact]
        public async Task UpdateProduct_changes_fields_and_regenerates_code()
        {
            var product = new Product("Casco viejo", "OLD-CODE", "REF1", "desc", true, 5m, 100m) { Id = 3 };
            var repository = new Mock<IProductsRepository>();
            repository.Setup(r => r.GetAsync(3)).ReturnsAsync(product);
            repository.Setup(r => r.GetByReference(It.IsAny<string>())).ReturnsAsync((Product?)null);

            var handler = new UpdateProductCommandHandler(repository.Object, _user.Object);
            var dto = new UpdateProductDto { Id = 3, Name = "Guantes Pro", Reference = "GP-10", Status = false };

            var result = await handler.Handle(new UpdateProductCommand(dto), CancellationToken.None);

            result.IsError.Should().BeFalse();
            product.Name.Should().Be("Guantes Pro");
            product.Status.Should().BeFalse();
            product.Code.Should().Be("EL-GUPR-GP10"); // regenerado por SetInternalCode
            repository.Verify(r => r.UpdateAsync(product), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_fails_when_product_missing()
        {
            var repository = new Mock<IProductsRepository>();
            repository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((Product?)null);

            var handler = new UpdateProductCommandHandler(repository.Object, _user.Object);
            var result = await handler.Handle(
                new UpdateProductCommand(new UpdateProductDto { Id = 99, Name = "X", Reference = "Y" }),
                CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Products.NotFound");
        }

        [Fact]
        public async Task UpdateProduct_fails_when_reference_belongs_to_another_product()
        {
            var product = new Product("A", "C", "REF1", null, true, 0m, 0m) { Id = 1 };
            var other = new Product("B", "C2", "REF2", null, true, 0m, 0m) { Id = 2 };
            var repository = new Mock<IProductsRepository>();
            repository.Setup(r => r.GetAsync(1)).ReturnsAsync(product);
            repository.Setup(r => r.GetByReference("REF2")).ReturnsAsync(other);

            var handler = new UpdateProductCommandHandler(repository.Object, _user.Object);
            var result = await handler.Handle(
                new UpdateProductCommand(new UpdateProductDto { Id = 1, Name = "A", Reference = "REF2" }),
                CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Products.AlreadyExists");
        }

        [Fact]
        public async Task UpdateThird_changes_fields()
        {
            var third = new Third("old@test.com", "111", "old", true, "300", "Old", "Name", 1) { Id = 5 };
            var repository = new Mock<IThirdRepository>();
            repository.Setup(r => r.GetAsync(5)).ReturnsAsync(third);
            repository.Setup(r => r.GetByIdentification(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Third?)null);

            var handler = new UpdateThirdCommandHandler(repository.Object, _user.Object);
            var dto = new UpdateThirdDto
            {
                Id = 5, Identification = "222", Name = "Nuevo", Surnames = "Cliente",
                CellPhone = "301", Address = "nueva", Email = "new@test.com", Active = false, TypeId = 2
            };

            var result = await handler.Handle(new UpdateThirdCommand(dto), CancellationToken.None);

            result.IsError.Should().BeFalse();
            third.Identification.Should().Be("222");
            third.Name.Should().Be("Nuevo");
            third.TypeId.Should().Be(2);
            third.Active.Should().BeFalse();
            repository.Verify(r => r.UpdateAsync(third), Times.Once);
        }

        [Fact]
        public async Task UpdateThird_fails_when_third_missing()
        {
            var repository = new Mock<IThirdRepository>();
            repository.Setup(r => r.GetAsync(It.IsAny<int>())).ReturnsAsync((Third?)null);

            var handler = new UpdateThirdCommandHandler(repository.Object, _user.Object);
            var result = await handler.Handle(
                new UpdateThirdCommand(new UpdateThirdDto { Id = 42, Identification = "1", TypeId = 1 }),
                CancellationToken.None);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("Thirds.NotFound");
        }
    }
}

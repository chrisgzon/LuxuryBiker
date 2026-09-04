using FluentAssertions;
using LuxuryBiker.Domain.Entities.Products;
using Xunit;

namespace LuxuryBiker.Application.UnitTests
{
    public class ProductTests
    {
        [Fact]
        public void Create_generates_the_internal_code()
        {
            var product = Product.Create("Casco LS2", "LS2-R5", "Integral", status: true);

            product.Code.Should().Be("EL-CALS-LS2R5");
            product.Stock.Should().Be(0m);
        }

        [Fact]
        public void Create_handles_names_with_single_letter_words()
        {
            // Antes lanzaba ArgumentOutOfRangeException al hacer Substring(0, 2).
            Action act = () => Product.Create("A Casco", "X-1", null, status: true);

            act.Should().NotThrow();
        }

        [Fact]
        public void Sell_refuses_to_leave_stock_negative()
        {
            var product = TestData.Product(id: 1, stock: 2m);

            Action act = () => product.Sell(3m);

            act.Should().Throw<InvalidOperationException>();
            product.Stock.Should().Be(2m);
        }

        [Fact]
        public void Sell_discounts_when_there_is_stock()
        {
            var product = TestData.Product(id: 1, stock: 5m);

            product.Sell(2m);

            product.Stock.Should().Be(3m);
        }

        [Fact]
        public void RevertPurchase_may_leave_stock_negative()
        {
            // La mercancía de la compra cancelada puede haberse vendido ya:
            // el descuadre debe quedar visible, no bloquearse.
            var product = TestData.Product(id: 1, stock: 1m);

            product.RevertPurchase(3m);

            product.Stock.Should().Be(-2m);
        }

        [Fact]
        public void UpdateDetails_regenerates_the_internal_code()
        {
            var product = Product.Create("Casco LS2", "LS2-R5", null, status: true);

            product.UpdateDetails("Guantes Pro", "GP-10", null, status: false);

            product.Code.Should().Be("EL-GUPR-GP10");
            product.Status.Should().BeFalse();
        }
    }
}

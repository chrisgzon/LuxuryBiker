using LuxuryBiker.Domain.Entities.Products;

namespace LuxuryBiker.Application.UnitTests
{
    internal static class TestData
    {
        public static Product Product(int id, decimal stock, decimal value = 0m) =>
            new("Producto " + id, code: null, reference: "REF" + id, description: null,
                status: true, stock: stock, value: value)
            {
                Id = id
            };
    }
}

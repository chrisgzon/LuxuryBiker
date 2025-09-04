using ErrorOr;

namespace LuxuryBiker.Domain.Entities.Products
{
    public class ProductsErrors
    {
        public static Error Exists { get; } = Error.Validation(
            code: "Products.AlreadyExists",
            description: "El Producto con la referencia ingresada ya se encuentra registrado en el sistema."
        );
    }
}

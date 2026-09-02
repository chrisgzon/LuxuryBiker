using ErrorOr;

namespace LuxuryBiker.Domain.Entities.Products
{
    public class ProductsErrors
    {
        public static Error Exists { get; } = Error.Validation(
            code: "Products.AlreadyExists",
            description: "El Producto con la referencia ingresada ya se encuentra registrado en el sistema."
        );

        public static Error NotFound { get; } = Error.NotFound(
            code: "Products.NotFound",
            description: "El producto indicado no existe."
        );
    }
}

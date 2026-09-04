using ErrorOr;

namespace LuxuryBiker.Domain.Entities.Purchases
{
    public static class PurchasesErrors
    {
        public static Error EmptyDetails { get; } = Error.Validation(
            code: "Purchases.EmptyDetails",
            description: "La compra debe incluir al menos un producto.");

        public static Error SupplierNotFound { get; } = Error.Validation(
            code: "Purchases.SupplierNotFound",
            description: "El proveedor seleccionado no existe.");

        public static Error ProductNotFound(int productId) => Error.Validation(
            code: "Purchases.ProductNotFound",
            description: $"El producto con id {productId} no existe o está inactivo.");

        public static Error NotFound { get; } = Error.NotFound(
            code: "Purchases.NotFound",
            description: "La compra indicada no existe.");
    }
}

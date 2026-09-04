using ErrorOr;

namespace LuxuryBiker.Domain.Entities.Sales
{
    public static class SalesErrors
    {
        public static Error EmptyDetails { get; } = Error.Validation(
            code: "Sales.EmptyDetails",
            description: "La venta debe incluir al menos un producto.");

        public static Error ClientNotFound { get; } = Error.Validation(
            code: "Sales.ClientNotFound",
            description: "El cliente seleccionado no existe.");

        public static Error ProductNotFound(int productId) => Error.Validation(
            code: "Sales.ProductNotFound",
            description: $"El producto con id {productId} no existe o está inactivo.");

        public static Error InsufficientStock(string productName, decimal available, decimal requested) => Error.Validation(
            code: "Sales.InsufficientStock",
            description: $"Stock insuficiente para '{productName}': disponible {available}, solicitado {requested}.");

        public static Error NotFound { get; } = Error.NotFound(
            code: "Sales.NotFound",
            description: "La venta indicada no existe.");
    }
}

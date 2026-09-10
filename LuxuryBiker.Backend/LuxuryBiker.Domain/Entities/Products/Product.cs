using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;

namespace LuxuryBiker.Domain.Entities.Products
{
    public class Product : BaseAuditableEntity<int>
    {
        private const string SeparatorCode = "-";
        private const string CompanyPrefix = "EL"; // iniciales de la empresa
        private const int TakeWords = 2;
        private const int TakeCharsPerWord = 2;

        public Product(string name, string? code, string reference, string? description, bool? status, decimal? stock, decimal? value)
        {
            Name = name;
            Code = code;
            Reference = reference;
            Description = description;
            Status = status;
            Stock = stock;
            Value = value;

            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }

        /// <summary>
        /// Crea un producto nuevo ya con su código interno calculado, de modo que nunca
        /// existe una instancia en estado inválido (sin código).
        /// </summary>
        public static Product Create(string name, string reference, string? description, bool? status)
        {
            var product = new Product(name, code: null, reference, description, status, stock: 0m, value: 0m);
            product.SetInternalCode();
            return product;
        }

        public string Name { get; private set; }
        public string? Code { get; private set; }
        public string Reference { get; private set; }
        public string? Description { get; private set; }
        public bool? Status { get; private set; }
        public decimal? Stock { get; private set; }
        public decimal? Value { get; private set; }

        public IEnumerable<PurchaseDetail>? Purchases { get; private set; }
        public IEnumerable<SaleDetail>? Sales { get; private set; }

        /// <summary>Actualiza los datos editables del producto y regenera el código interno.</summary>
        public void UpdateDetails(string name, string reference, string? description, bool? status)
        {
            Name = name;
            Reference = reference;
            Description = description;
            Status = status;
            SetInternalCode();
            LastModified = DateTime.Now;
        }

        /// <summary>Indica si hay inventario suficiente para despachar la cantidad pedida.</summary>
        public bool HasStockFor(decimal quantity) => (Stock ?? 0m) >= quantity;

        /// <summary>Aumenta el inventario disponible (registro de una compra validada).</summary>
        public void IncreaseStock(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor que cero.");

            Stock = (Stock ?? 0) + quantity;
            LastModified = DateTime.Now;
        }

        /// <summary>Disminuye el inventario disponible (registro de una venta validada).</summary>
        public void DecreaseStock(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor que cero.");

            Stock = (Stock ?? 0) - quantity;
            LastModified = DateTime.Now;
        }

        /// <summary>Registra el último valor de compra del producto.</summary>
        public void RegisterPurchaseValue(decimal value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "El valor no puede ser negativo.");

            Value = value;
            LastModified = DateTime.Now;
        }

        /// <summary>
        /// Código interno: prefijo de empresa + iniciales del nombre + referencia.
        /// Ej.: "Casco LS2" con referencia "LS2-R5" -> "EL-CALS-LS2R5".
        /// </summary>
        private void SetInternalCode()
        {
            string initialCode = $"{CompanyPrefix}{SeparatorCode}";

            string[] wordsName = Name.ToUpper().Split(" ");
            if (wordsName.Length > TakeWords)
            {
                wordsName = wordsName.Take(TakeWords).ToArray();
            }

            foreach (var word in wordsName)
            {
                initialCode += word.Length >= TakeCharsPerWord
                    ? word.Substring(0, TakeCharsPerWord)
                    : word;
            }

            string normalizedReference = Reference
                .Replace(" ", string.Empty)
                .Replace(SeparatorCode, string.Empty)
                .ToUpper();

            Code = $"{initialCode}{SeparatorCode}{normalizedReference}";
        }
    }
}

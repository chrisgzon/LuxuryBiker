using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;

namespace LuxuryBiker.Domain.Entities.Products
{
    public class Product : BaseAuditableEntity<int>
    {
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

        public string Name { get; private set; }
        public string? Code { get; private set; }
        public string Reference { get; private set; }
        public string? Description { get; private set; }
        public bool? Status { get; private set; }
        public decimal? Stock { get; private set; }
        public decimal? Value { get; private set; }

        public IEnumerable<PurchaseDetail>? Purchases { get; private set; }
        public IEnumerable<SaleDetail>? Sales { get; private set; }

        private readonly string SeparatorCode = "-";
        private readonly int TakeWords = 2;
        private readonly int TakeWordPerWords = 2;

        public void SetInternalCode()
        {
            string initialCode = $"EL{SeparatorCode}"; // in case that manage many enterprises, should be initial's name company

            string[] wordsName = this.Name.ToUpper().Split(" ");
            if (wordsName.Length > TakeWords)
            {
                wordsName = wordsName.Take(TakeWords).ToArray();
            }

            foreach (var word in wordsName)
            {
                initialCode += word.Substring(0, TakeWordPerWords);
            }

            this.Code = $"{initialCode}{SeparatorCode}{this.Reference.Replace(" ", String.Empty).Replace(SeparatorCode, string.Empty).ToUpper()}";
        }

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
    }
}

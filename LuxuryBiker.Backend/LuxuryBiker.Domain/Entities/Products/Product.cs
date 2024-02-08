using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Data.Entities.Productos
{
    public class Product : BaseAuditableEntity<int>
    {
        public string? Name { get; private set; }
        public string? Code { get; private set; }
        public string? Reference { get; private set; }
        public string? Description { get; private set; }
        public bool State { get; private set; }
        public decimal Stock { get; private set; }
        public decimal Value { get; private set; }
    }
}

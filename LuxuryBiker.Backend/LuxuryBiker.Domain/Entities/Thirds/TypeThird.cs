using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Data.Entities.Terceros
{
    public class TypeThird : BaseEntity<int>
    {
        public string? Name { get; set; }
        public bool Active { get; set; }
    }
}

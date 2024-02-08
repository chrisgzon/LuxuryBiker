using LuxuryBiker.Domain.Entities.Common;

namespace LuxuryBiker.Domain.Entities.Thirds
{
    public class TypeThird : BaseEntity<int>
    {
        public string? Name { get; set; }
        public bool Active { get; set; }
    }
}

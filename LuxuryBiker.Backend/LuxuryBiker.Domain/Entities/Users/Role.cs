using LuxuryBiker.Domain.Entities.Common;

namespace LuxuryBiker.Domain.Entities.Users
{
    public class Role : BaseEntity<int>
    {
        public string? Name { get; set; }
        public bool Active { get; set; }
    }
}

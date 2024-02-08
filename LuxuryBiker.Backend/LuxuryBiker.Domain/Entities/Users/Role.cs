using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Data.Entities.Users
{
    public class Role : BaseEntity<int>
    {
        public string? Name { get; set; }
        public bool Active { get; set; }
    }
}

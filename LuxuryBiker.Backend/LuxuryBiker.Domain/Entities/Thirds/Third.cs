using LuxuryBiker.Domain.Entities.Common;
using LuxuryBiker.Domain.Entities.Purchases;
using LuxuryBiker.Domain.Entities.Sales;

namespace LuxuryBiker.Domain.Entities.Thirds
{
    public class Third : BaseAuditableEntity<int>
    {
        public Third(string? email, string identification, string? address, bool? active, string? cellPhone, string? name, string? surnames, int typeId)
        {
            Email = email;
            Identification = identification;
            Address = address;
            Active = active;
            CellPhone = cellPhone;
            Name = name;
            Surnames = surnames;
            TypeId = typeId;

            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public string? Email { get; private set; }
        public string Identification { get; private set; }
        public string? Address { get; private set; }
        public bool? Active { get; private set; }
        public string? CellPhone { get; private set; }
        public string? Name { get; private set; }
        public string? Surnames { get; private set; }

        public int TypeId { get; private set; }
        public TypeThird? Type { get; set; }
        public IEnumerable<Sale>? Purchases { get; set; }
        public IEnumerable<Purchase>? Sales { get; set; }

        /// <summary>Actualiza los datos editables del tercero.</summary>
        public void Update(string? email, string identification, string? address, bool? active,
            string? cellPhone, string? name, string? surnames, int typeId)
        {
            Email = email;
            Identification = identification;
            Address = address;
            Active = active;
            CellPhone = cellPhone;
            Name = name;
            Surnames = surnames;
            TypeId = typeId;
            LastModified = DateTime.Now;
        }
    }
}

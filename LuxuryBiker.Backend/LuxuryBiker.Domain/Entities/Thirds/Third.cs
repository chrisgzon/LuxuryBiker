﻿using LuxuryBiker.Domain.Common;

namespace LuxuryBiker.Data.Entities.Terceros
{
    public class Third : BaseAuditableEntity<int>
    {
        public string? Email { get; set; }
        public string? Identification { get; set; }
        public string? Address { get; set; }
        public bool Active { get; set; }
        public int TypeId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Surnames { get; set; }

        public TypeThird Type { get; set; }
    }
}

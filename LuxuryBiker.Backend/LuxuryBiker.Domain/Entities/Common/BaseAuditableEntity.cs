﻿namespace LuxuryBiker.Domain.Entities.Common
{
    public abstract class BaseAuditableEntity<T> : BaseEntity<T>
    {
        public DateTimeOffset Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}

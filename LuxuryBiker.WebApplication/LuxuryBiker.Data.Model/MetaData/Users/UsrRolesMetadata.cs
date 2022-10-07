using LuxuryBiker.Data.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Users
{
    class UsrRolesMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<UsrRoles> entityBuilder)
        {
            entityBuilder.ToTable("UsrRoles");
            entityBuilder.HasKey(x => x.IdRol);
            entityBuilder.Property(x => x.NombreRol).IsRequired().HasMaxLength(60);
            entityBuilder.Property(x => x.SenActivo).IsRequired().HasDefaultValue(0);
        }
    }
}

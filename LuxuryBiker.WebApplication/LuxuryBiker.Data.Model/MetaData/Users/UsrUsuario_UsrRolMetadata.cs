using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Users
{
    class UsrUsuario_UsrRolMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Entities.Users.UsrUsuario_UsrRol> entityBuilder)
        {
            entityBuilder.ToTable("UsrUsuario_UsrRol");
            entityBuilder.HasKey(x=>x.Id);
            entityBuilder.Property(x => x.RolIdRol).IsRequired();
            entityBuilder.Property(x => x.UsuarioIdUsuario).HasMaxLength(60).IsRequired();

            entityBuilder.HasOne(x => x.Usuario).WithMany(x => x.Roles);
            entityBuilder.HasOne(x => x.Rol).WithMany(x => x.Usuarios);
        }
    }
}

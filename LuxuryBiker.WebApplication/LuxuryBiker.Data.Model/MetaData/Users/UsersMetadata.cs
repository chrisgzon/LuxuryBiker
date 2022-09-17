using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LuxuryBiker.Data.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Users
{
    public class UsersMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Entities.Users.Users> entityBuilder)
        {
            entityBuilder.ToTable("Users");
            entityBuilder.HasKey(x=>x.IdUsuario);
            entityBuilder.Property(x => x.IdUsuario).HasMaxLength(60).IsRequired();

            entityBuilder.Property(x=>x.Nombres).HasMaxLength(250).IsRequired();
            entityBuilder.Property(x => x.Apellidos).HasMaxLength(250);
            entityBuilder.Property(x => x.Identificacion).HasMaxLength(60).IsRequired();
            entityBuilder.Property(x => x.Email).HasMaxLength(100);
            entityBuilder.Property(x => x.FechaNacimiento);
            entityBuilder.Property(x => x.FechaCreacion).IsRequired();
            entityBuilder.Property(x => x.PasswordHash).HasMaxLength(800).IsRequired();
            entityBuilder.Property(x => x.PhoneNumber).HasMaxLength(60);
            entityBuilder.Property(x => x.UserName).HasMaxLength(250).IsRequired();
            entityBuilder.Property(x => x.SenActivo).IsRequired().HasDefaultValue(0);

            entityBuilder.HasMany(x => x.Roles).WithOne(x=>x.Usuario);
        }
    }
}

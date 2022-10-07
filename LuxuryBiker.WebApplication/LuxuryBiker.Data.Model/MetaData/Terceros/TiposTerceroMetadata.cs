using LuxuryBiker.Data.Entities.Terceros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Terceros
{
    class TiposTerceroMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<TiposTercero> entityBuilder)
        {
            entityBuilder.ToTable("TiposTercero").HasKey(x=>x.IdTipo);
            entityBuilder.Property(x => x.Nombre).IsRequired().HasMaxLength(60);
            entityBuilder.Property(x => x.SenActivo).IsRequired().HasDefaultValue(1);

            entityBuilder.HasMany(x => x.Terceros).WithOne(x => x.Tipo);
        }
    }
}

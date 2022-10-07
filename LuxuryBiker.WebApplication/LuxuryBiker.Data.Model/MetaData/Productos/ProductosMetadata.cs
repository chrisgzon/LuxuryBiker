using LuxuryBiker.Data.Entities.Productos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Productos
{
    class ProductosMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Producto> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Productos").HasKey(x => x.IdProducto);
            entityTypeBuilder.Property(x => x.Codigo).HasMaxLength(60).IsRequired();
            entityTypeBuilder.Property(x => x.Estado).IsRequired().HasDefaultValue(1);
            entityTypeBuilder.Property(x => x.FechaRegistro).IsRequired();
            entityTypeBuilder.Property(x => x.Nombre).IsRequired().HasMaxLength(60);
            entityTypeBuilder.Property(x => x.Descripcion).HasMaxLength(500);
            entityTypeBuilder.Property(x => x.Stock).HasPrecision(10, 3);
            entityTypeBuilder.Property(x => x.ValorProducto).HasPrecision(28, 6);
        }
    }
}

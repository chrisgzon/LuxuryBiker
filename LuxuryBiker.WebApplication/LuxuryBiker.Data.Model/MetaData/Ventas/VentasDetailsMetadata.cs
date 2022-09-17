using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Ventas
{
    class VentasDetailsMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Entities.Ventas.VentasDetails> entityBuilder)
        {
            entityBuilder.ToTable("VentasDetails").HasKey(x=>x.Id);
            entityBuilder.Property(x => x.Cantidad).IsRequired().HasPrecision(10, 3);
            entityBuilder.Property(x => x.VentaIdVenta).IsRequired();
            entityBuilder.Property(x => x.ProductoIdProducto).IsRequired();
            entityBuilder.Property(x => x.ValorProducto).IsRequired().HasPrecision(28, 6);

            entityBuilder.HasOne(x => x.Venta).WithMany(x => x.DetallesVentas);
            entityBuilder.HasOne(x => x.Producto).WithMany(x => x.DetallesVenta);
        }
    }
}

using LuxuryBiker.Data.Entities.Compras;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Compras
{
    class ComprasDetailsMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<ComprasDetails> entityBuilder)
        {
            entityBuilder.ToTable("ComprasDetails").HasKey(x => x.Id);
            entityBuilder.Property(x=>x.cantidad).IsRequired().HasPrecision(10, 3);
            entityBuilder.Property(x => x.CompraIdCompra).IsRequired();
            entityBuilder.Property(x => x.ProductoIdProducto).IsRequired();
            entityBuilder.Property(x => x.ValorProducto).IsRequired().HasPrecision(28,6);
            
            entityBuilder.HasOne(x => x.Compra).WithMany(x=>x.DetallesCompra);
            entityBuilder.HasOne(x => x.Producto).WithMany(x => x.DetallesCompra);
        }
    }
}

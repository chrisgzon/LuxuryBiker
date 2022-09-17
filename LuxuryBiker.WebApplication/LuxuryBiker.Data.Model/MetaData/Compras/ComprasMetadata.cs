using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Compras
{
    class ComprasMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Entities.Compras.Compras> entityBuilder)
        {
            // En esta tabla se registran las compras que realiza la empresa para surtir su inventario
            entityBuilder.ToTable("Compras").HasKey(x => x.IdCompra);
            entityBuilder.Property(x => x.CodCompra).HasMaxLength(60).IsRequired();
            entityBuilder.Property(x => x.Estado).IsRequired().HasDefaultValue(1);
            entityBuilder.Property(x => x.FechaCompra).IsRequired();
            entityBuilder.Property(x => x.TerceroIdTercero).IsRequired();
            entityBuilder.Property(x => x.UsuarioIdUsuario).IsRequired();
            entityBuilder.Property(x => x.Total).HasPrecision(28,6);

            entityBuilder.HasOne(x => x.Usuario).WithMany(x => x.Compras);
            entityBuilder.HasOne(x => x.Tercero).WithMany(x=>x.VentasProveedor);
        }
    }
}

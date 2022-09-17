using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Ventas
{
    class VentasMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Entities.Ventas.Ventas> entityBuilder)
        {
            // En esta tabla se registran las ventas que realiza la empresa a sus clientes
            entityBuilder.ToTable("Ventas").HasKey(x => x.IdVenta);
            entityBuilder.Property(x => x.CodVenta).HasMaxLength(60).IsRequired();
            entityBuilder.Property(x => x.Estado).IsRequired().HasDefaultValue(1);
            entityBuilder.Property(x => x.FechaVenta).IsRequired();
            entityBuilder.Property(x => x.UsuarioIdUsuario).IsRequired();
            entityBuilder.Property(x => x.TerceroIdTercero);
            entityBuilder.Property(x => x.Total).HasPrecision(28, 6);

            entityBuilder.HasOne(x => x.Usuario).WithMany(x => x.Ventas);
            entityBuilder.HasOne(x => x.Tercero).WithMany(x => x.ComprasCliente);
        }
    }
}

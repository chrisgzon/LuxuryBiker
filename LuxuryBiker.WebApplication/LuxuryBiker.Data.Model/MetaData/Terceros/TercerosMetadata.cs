﻿using LuxuryBiker.Data.Entities.Terceros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuxuryBiker.Data.Model.MetaData.Terceros
{
    class TercerosMetadata
    {
        public static void SetEntityBuilder(EntityTypeBuilder<Tercero> entityBuilder)
        {
            entityBuilder.ToTable("Terceros").HasKey(x => x.IdTercero);

            entityBuilder.Property(x => x.Direccion).HasMaxLength(200);
            entityBuilder.Property(x => x.Email).HasMaxLength(150);
            entityBuilder.Property(x => x.FechaCreacion).IsRequired();
            entityBuilder.Property(x => x.Identificacion).HasMaxLength(60).IsRequired();
            entityBuilder.Property(x => x.TipoIdTipo).IsRequired();
            entityBuilder.Property(x => x.Nombres).IsRequired().HasMaxLength(250);
            entityBuilder.Property(x => x.Apellidos).HasMaxLength(250);
            entityBuilder.Property(x => x.Celular).HasMaxLength(50);

            entityBuilder.HasOne(x => x.Tipo).WithMany(x=>x.Terceros);
        }
    }
}

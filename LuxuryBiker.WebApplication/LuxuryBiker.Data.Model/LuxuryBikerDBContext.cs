using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LuxuryBiker.Data.Entities.Users;
using LuxuryBiker.Data.Entities.Compras;
using LuxuryBiker.Data.Entities.Productos;
using LuxuryBiker.Data.Entities.Terceros;
using LuxuryBiker.Data.Entities.Ventas;
using LuxuryBiker.Data.Model.MetaData.Users;
using LuxuryBiker.Data.Model.MetaData.Compras;
using LuxuryBiker.Data.Model.MetaData.Ventas;
using LuxuryBiker.Data.Model.MetaData.Terceros;
using LuxuryBiker.Data.Model.MetaData.Productos;

namespace LuxuryBiker.Data.Model
{
    public class LuxuryBikerDBContext : DbContext
    {
        #region Users
        public DbSet<Users> Users { get; set; }
        public DbSet<UsrRoles> Roles { get; set; }
        public DbSet<UsrUsuario_UsrRol> UsrUsuarios_UsrRoles { get; set; }
        #endregion
        #region Compras
        public DbSet<Compras> Compras { get; set; }
        public DbSet<ComprasDetails> DetailsCompras { get; set; }
        #endregion
        #region Productos
        DbSet<Productos> Productos { get; set; }
        #endregion
        #region Terceros
        public DbSet<Terceros> Terceros { get; set; }
        public DbSet<TiposTercero> TiposTercero { get; set; }
        #endregion
        #region Ventas
        public DbSet<Ventas> Ventas { get; set; }
        public DbSet<VentasDetails> DetailsVentas { get; set; }
        #endregion
        public LuxuryBikerDBContext(DbContextOptions options) : base(options) { }
        public LuxuryBikerDBContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region UsersMetadata
            UsersMetadata.SetEntityBuilder(modelBuilder.Entity<Users>());
            UsrRolesMetadata.SetEntityBuilder(modelBuilder.Entity<UsrRoles>());
            UsrUsuario_UsrRolMetadata.SetEntityBuilder(modelBuilder.Entity<UsrUsuario_UsrRol>());
            #endregion
            #region Terceros
            TercerosMetadata.SetEntityBuilder(modelBuilder.Entity<Terceros>());
            TiposTerceroMetadata.SetEntityBuilder(modelBuilder.Entity<TiposTercero>());
            #endregion
            #region Compras
            ComprasMetadata.SetEntityBuilder(modelBuilder.Entity<Compras>());
            ComprasDetailsMetadata.SetEntityBuilder(modelBuilder.Entity<ComprasDetails>());
            #endregion
            #region Ventas
            VentasMetadata.SetEntityBuilder(modelBuilder.Entity<Ventas>());
            VentasDetailsMetadata.SetEntityBuilder(modelBuilder.Entity<VentasDetails>());
            #endregion
            #region Productos
            ProductosMetadata.SetEntityBuilder(modelBuilder.Entity<Productos>());
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-TERFBNJ\\MSSQLSERVER2019;Initial Catalog=LuxuryBiker;User ID=sa;Password=D@v1d*21;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}

﻿// <auto-generated />
using System;
using LuxuryBiker.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LuxuryBiker.Data.Model.Migrations
{
    [DbContext(typeof(LuxuryBikerDBContext))]
    partial class LuxuryBikerDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Compras.Compras", b =>
                {
                    b.Property<int>("IdCompra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CodCompra")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("FechaCompra")
                        .HasColumnType("datetime2");

                    b.Property<int>("TerceroIdTercero")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasPrecision(28, 6)
                        .HasColumnType("decimal(28,6)");

                    b.Property<string>("UsuarioIdUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("IdCompra");

                    b.HasIndex("TerceroIdTercero");

                    b.HasIndex("UsuarioIdUsuario");

                    b.ToTable("Compras");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Compras.ComprasDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompraIdCompra")
                        .HasColumnType("int");

                    b.Property<int>("ProductoIdProducto")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorProducto")
                        .HasPrecision(28, 6)
                        .HasColumnType("decimal(28,6)");

                    b.Property<decimal>("cantidad")
                        .HasPrecision(10, 3)
                        .HasColumnType("decimal(10,3)");

                    b.HasKey("Id");

                    b.HasIndex("CompraIdCompra");

                    b.HasIndex("ProductoIdProducto");

                    b.ToTable("ComprasDetails");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Productos.Productos", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<decimal>("Stock")
                        .HasPrecision(10, 3)
                        .HasColumnType("decimal(10,3)");

                    b.Property<decimal>("ValorProducto")
                        .HasPrecision(28, 6)
                        .HasColumnType("decimal(28,6)");

                    b.HasKey("IdProducto");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Terceros.Terceros", b =>
                {
                    b.Property<int>("IdTercero")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Direccion")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("Identificacion")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("SenActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("TipoIdTipo")
                        .HasColumnType("int");

                    b.HasKey("IdTercero");

                    b.HasIndex("TipoIdTipo");

                    b.ToTable("Terceros");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Terceros.TiposTercero", b =>
                {
                    b.Property<int>("IdTipo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("SenActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.HasKey("IdTipo");

                    b.ToTable("TiposTercero");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Users.Users", b =>
                {
                    b.Property<string>("IdUsuario")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Apellidos")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Identificacion")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(800)
                        .HasColumnType("nvarchar(800)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("SenActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("IdUsuario");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Users.UsrRoles", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NombreRol")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("SenActivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("IdRol");

                    b.ToTable("UsrRoles");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Users.UsrUsuario_UsrRol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RolIdRol")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioIdUsuario")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.HasIndex("RolIdRol");

                    b.HasIndex("UsuarioIdUsuario");

                    b.ToTable("UsrUsuario_UsrRol");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Ventas.Ventas", b =>
                {
                    b.Property<int>("IdVenta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CodVenta")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<DateTime>("FechaVenta")
                        .HasColumnType("datetime2");

                    b.Property<int>("TerceroIdTercero")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasPrecision(28, 6)
                        .HasColumnType("decimal(28,6)");

                    b.Property<string>("UsuarioIdUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("IdVenta");

                    b.HasIndex("TerceroIdTercero");

                    b.HasIndex("UsuarioIdUsuario");

                    b.ToTable("Ventas");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Ventas.VentasDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Cantidad")
                        .HasPrecision(10, 3)
                        .HasColumnType("decimal(10,3)");

                    b.Property<int>("ProductoIdProducto")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorProducto")
                        .HasPrecision(28, 6)
                        .HasColumnType("decimal(28,6)");

                    b.Property<int>("VentaIdVenta")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductoIdProducto");

                    b.HasIndex("VentaIdVenta");

                    b.ToTable("VentasDetails");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Compras.Compras", b =>
                {
                    b.HasOne("LuxuryBiker.Data.Entities.Terceros.Terceros", "Tercero")
                        .WithMany("VentasProveedor")
                        .HasForeignKey("TerceroIdTercero")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LuxuryBiker.Data.Entities.Users.Users", "Usuario")
                        .WithMany("Compras")
                        .HasForeignKey("UsuarioIdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tercero");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Compras.ComprasDetails", b =>
                {
                    b.HasOne("LuxuryBiker.Data.Entities.Compras.Compras", "Compra")
                        .WithMany("DetallesCompra")
                        .HasForeignKey("CompraIdCompra")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LuxuryBiker.Data.Entities.Productos.Productos", "Producto")
                        .WithMany("DetallesCompra")
                        .HasForeignKey("ProductoIdProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Compra");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Terceros.Terceros", b =>
                {
                    b.HasOne("LuxuryBiker.Data.Entities.Terceros.TiposTercero", "Tipo")
                        .WithMany("Terceros")
                        .HasForeignKey("TipoIdTipo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tipo");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Users.UsrUsuario_UsrRol", b =>
                {
                    b.HasOne("LuxuryBiker.Data.Entities.Users.UsrRoles", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolIdRol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LuxuryBiker.Data.Entities.Users.Users", "Usuario")
                        .WithMany("Roles")
                        .HasForeignKey("UsuarioIdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Ventas.Ventas", b =>
                {
                    b.HasOne("LuxuryBiker.Data.Entities.Terceros.Terceros", "Tercero")
                        .WithMany("ComprasCliente")
                        .HasForeignKey("TerceroIdTercero")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LuxuryBiker.Data.Entities.Users.Users", "Usuario")
                        .WithMany("Ventas")
                        .HasForeignKey("UsuarioIdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tercero");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Ventas.VentasDetails", b =>
                {
                    b.HasOne("LuxuryBiker.Data.Entities.Productos.Productos", "Producto")
                        .WithMany("DetallesVenta")
                        .HasForeignKey("ProductoIdProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LuxuryBiker.Data.Entities.Ventas.Ventas", "Venta")
                        .WithMany("DetallesVentas")
                        .HasForeignKey("VentaIdVenta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("Venta");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Compras.Compras", b =>
                {
                    b.Navigation("DetallesCompra");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Productos.Productos", b =>
                {
                    b.Navigation("DetallesCompra");

                    b.Navigation("DetallesVenta");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Terceros.Terceros", b =>
                {
                    b.Navigation("ComprasCliente");

                    b.Navigation("VentasProveedor");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Terceros.TiposTercero", b =>
                {
                    b.Navigation("Terceros");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Users.Users", b =>
                {
                    b.Navigation("Compras");

                    b.Navigation("Roles");

                    b.Navigation("Ventas");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Users.UsrRoles", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("LuxuryBiker.Data.Entities.Ventas.Ventas", b =>
                {
                    b.Navigation("DetallesVentas");
                });
#pragma warning restore 612, 618
        }
    }
}

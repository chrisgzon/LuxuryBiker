using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LuxuryBiker.Data.Model.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Stock = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    ValorProducto = table.Column<decimal>(type: "decimal(28,6)", precision: 28, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IdProducto);
                });

            migrationBuilder.CreateTable(
                name: "TiposTercero",
                columns: table => new
                {
                    IdTipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SenActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTercero", x => x.IdTipo);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUsuario = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SenActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "UsrRoles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    SenActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsrRoles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Terceros",
                columns: table => new
                {
                    IdTercero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Identificacion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SenActivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TipoIdTipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terceros", x => x.IdTercero);
                    table.ForeignKey(
                        name: "FK_Terceros_TiposTercero_TipoIdTipo",
                        column: x => x.TipoIdTipo,
                        principalTable: "TiposTercero",
                        principalColumn: "IdTipo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsrUsuario_UsrRol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioIdUsuario = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    RolIdRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsrUsuario_UsrRol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsrUsuario_UsrRol_Users_UsuarioIdUsuario",
                        column: x => x.UsuarioIdUsuario,
                        principalTable: "Users",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsrUsuario_UsrRol_UsrRoles_RolIdRol",
                        column: x => x.RolIdRol,
                        principalTable: "UsrRoles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodCompra = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioIdUsuario = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    TerceroIdTercero = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(28,6)", precision: 28, scale: 6, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.IdCompra);
                    table.ForeignKey(
                        name: "FK_Compras_Terceros_TerceroIdTercero",
                        column: x => x.TerceroIdTercero,
                        principalTable: "Terceros",
                        principalColumn: "IdTercero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_Users_UsuarioIdUsuario",
                        column: x => x.UsuarioIdUsuario,
                        principalTable: "Users",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    IdVenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodVenta = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    FechaVenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerceroIdTercero = table.Column<int>(type: "int", nullable: false),
                    UsuarioIdUsuario = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Total = table.Column<decimal>(type: "decimal(28,6)", precision: 28, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.IdVenta);
                    table.ForeignKey(
                        name: "FK_Ventas_Terceros_TerceroIdTercero",
                        column: x => x.TerceroIdTercero,
                        principalTable: "Terceros",
                        principalColumn: "IdTercero",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ventas_Users_UsuarioIdUsuario",
                        column: x => x.UsuarioIdUsuario,
                        principalTable: "Users",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComprasDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompraIdCompra = table.Column<int>(type: "int", nullable: false),
                    ProductoIdProducto = table.Column<int>(type: "int", nullable: false),
                    ValorProducto = table.Column<decimal>(type: "decimal(28,6)", precision: 28, scale: 6, nullable: false),
                    cantidad = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComprasDetails_Compras_CompraIdCompra",
                        column: x => x.CompraIdCompra,
                        principalTable: "Compras",
                        principalColumn: "IdCompra",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComprasDetails_Productos_ProductoIdProducto",
                        column: x => x.ProductoIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VentasDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoIdProducto = table.Column<int>(type: "int", nullable: false),
                    VentaIdVenta = table.Column<int>(type: "int", nullable: false),
                    ValorProducto = table.Column<decimal>(type: "decimal(28,6)", precision: 28, scale: 6, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentasDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentasDetails_Productos_ProductoIdProducto",
                        column: x => x.ProductoIdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VentasDetails_Ventas_VentaIdVenta",
                        column: x => x.VentaIdVenta,
                        principalTable: "Ventas",
                        principalColumn: "IdVenta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compras_TerceroIdTercero",
                table: "Compras",
                column: "TerceroIdTercero");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_UsuarioIdUsuario",
                table: "Compras",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasDetails_CompraIdCompra",
                table: "ComprasDetails",
                column: "CompraIdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasDetails_ProductoIdProducto",
                table: "ComprasDetails",
                column: "ProductoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Terceros_TipoIdTipo",
                table: "Terceros",
                column: "TipoIdTipo");

            migrationBuilder.CreateIndex(
                name: "IX_UsrUsuario_UsrRol_RolIdRol",
                table: "UsrUsuario_UsrRol",
                column: "RolIdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsrUsuario_UsrRol_UsuarioIdUsuario",
                table: "UsrUsuario_UsrRol",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_TerceroIdTercero",
                table: "Ventas",
                column: "TerceroIdTercero");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_UsuarioIdUsuario",
                table: "Ventas",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_VentasDetails_ProductoIdProducto",
                table: "VentasDetails",
                column: "ProductoIdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_VentasDetails_VentaIdVenta",
                table: "VentasDetails",
                column: "VentaIdVenta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprasDetails");

            migrationBuilder.DropTable(
                name: "UsrUsuario_UsrRol");

            migrationBuilder.DropTable(
                name: "VentasDetails");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "UsrRoles");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "Terceros");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TiposTercero");
        }
    }
}

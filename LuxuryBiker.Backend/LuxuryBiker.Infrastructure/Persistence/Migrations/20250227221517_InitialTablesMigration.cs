using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxuryBiker.Infrastructure.Persistence.Migrations
{
    public partial class InitialTablesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaNacimiento",
                table: "AspNetUsers",
                newName: "DateBirth");

            migrationBuilder.CreateTable(
                name: "T_PRODUCTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Stock = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PRODUCTS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeThird",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeThird", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_THIRDS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Identification = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CellPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Surnames = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_THIRDS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_THIRDS_TypeThird_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TypeThird",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_PURCHASES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DatePurchase = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ThirdId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PURCHASES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_PURCHASES_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PURCHASES_T_THIRDS_ThirdId",
                        column: x => x.ThirdId,
                        principalTable: "T_THIRDS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_SALES",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ThirdId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Total = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SALES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_SALES_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SALES_T_THIRDS_ThirdId",
                        column: x => x.ThirdId,
                        principalTable: "T_THIRDS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_PURCHASE_DETAILS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductValue = table.Column<decimal>(type: "decimal(28,6)", precision: 28, scale: 6, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PURCHASE_DETAILS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_PURCHASE_DETAILS_T_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "T_PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PURCHASE_DETAILS_T_PURCHASES_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "T_PURCHASES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_SALE_DETAILS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    ProductValue = table.Column<decimal>(type: "decimal(28,2)", precision: 28, scale: 2, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SALE_DETAILS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_SALE_DETAILS_T_PRODUCTS_ProductId",
                        column: x => x.ProductId,
                        principalTable: "T_PRODUCTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SALE_DETAILS_T_SALES_SaleId",
                        column: x => x.SaleId,
                        principalTable: "T_SALES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_PURCHASE_DETAILS_ProductId",
                table: "T_PURCHASE_DETAILS",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PURCHASE_DETAILS_PurchaseId",
                table: "T_PURCHASE_DETAILS",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PURCHASES_ThirdId",
                table: "T_PURCHASES",
                column: "ThirdId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PURCHASES_UserId",
                table: "T_PURCHASES",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_SALE_DETAILS_ProductId",
                table: "T_SALE_DETAILS",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_T_SALE_DETAILS_SaleId",
                table: "T_SALE_DETAILS",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_T_SALES_ThirdId",
                table: "T_SALES",
                column: "ThirdId");

            migrationBuilder.CreateIndex(
                name: "IX_T_SALES_UserId",
                table: "T_SALES",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_THIRDS_TypeId",
                table: "T_THIRDS",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_PURCHASE_DETAILS");

            migrationBuilder.DropTable(
                name: "T_SALE_DETAILS");

            migrationBuilder.DropTable(
                name: "T_PURCHASES");

            migrationBuilder.DropTable(
                name: "T_PRODUCTS");

            migrationBuilder.DropTable(
                name: "T_SALES");

            migrationBuilder.DropTable(
                name: "T_THIRDS");

            migrationBuilder.DropTable(
                name: "TypeThird");

            migrationBuilder.RenameColumn(
                name: "DateBirth",
                table: "AspNetUsers",
                newName: "FechaNacimiento");
        }
    }
}

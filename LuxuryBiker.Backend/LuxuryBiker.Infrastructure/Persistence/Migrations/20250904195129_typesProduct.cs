using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxuryBiker.Infrastructure.Persistence.Migrations
{
    public partial class typesProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_THIRDS_TypeThird_TypeId",
                table: "T_THIRDS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeThird",
                table: "TypeThird");

            migrationBuilder.RenameTable(
                name: "TypeThird",
                newName: "T_TYPE_THIRD");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "T_PRODUCTS",
                type: "decimal(28,2)",
                precision: 28,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(28,2)",
                oldPrecision: 28,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Stock",
                table: "T_PRODUCTS",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "T_PRODUCTS",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_TYPE_THIRD",
                table: "T_TYPE_THIRD",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_THIRDS_T_TYPE_THIRD_TypeId",
                table: "T_THIRDS",
                column: "TypeId",
                principalTable: "T_TYPE_THIRD",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_THIRDS_T_TYPE_THIRD_TypeId",
                table: "T_THIRDS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_TYPE_THIRD",
                table: "T_TYPE_THIRD");

            migrationBuilder.RenameTable(
                name: "T_TYPE_THIRD",
                newName: "TypeThird");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "T_PRODUCTS",
                type: "decimal(28,2)",
                precision: 28,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(28,2)",
                oldPrecision: 28,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Stock",
                table: "T_PRODUCTS",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "T_PRODUCTS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeThird",
                table: "TypeThird",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_THIRDS_TypeThird_TypeId",
                table: "T_THIRDS",
                column: "TypeId",
                principalTable: "TypeThird",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

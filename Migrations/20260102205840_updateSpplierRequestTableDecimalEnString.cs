using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_c_charp.Migrations
{
    /// <inheritdoc />
    public partial class updateSpplierRequestTableDecimalEnString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductTypes",
                table: "SupplierRequest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProductTypes",
                table: "SupplierRequest",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}

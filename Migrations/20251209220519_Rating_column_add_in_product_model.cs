using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_c_charp.Migrations
{
    /// <inheritdoc />
    public partial class Rating_column_add_in_product_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Product");
        }
    }
}

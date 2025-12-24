using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_c_charp.Migrations
{
    /// <inheritdoc />
    public partial class ajout_Is_Feature_Column_In_Product_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Product");
        }
    }
}

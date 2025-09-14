using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_IsHasVariants_Field_For_Product_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHasVariants",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHasVariants",
                table: "Product");
        }
    }
}

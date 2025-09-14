using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Banner_And_VideoUrl_For_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Product");
        }
    }
}

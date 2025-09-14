using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductColor_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductColorId",
                table: "OrderItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductColor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductColor_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductColorId",
                table: "OrderItem",
                column: "ProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_ProductId",
                table: "ProductColor",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductColor_ProductColorId",
                table: "OrderItem",
                column: "ProductColorId",
                principalTable: "ProductColor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductColor_ProductColorId",
                table: "OrderItem");

            migrationBuilder.DropTable(
                name: "ProductColor");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ProductColorId",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ProductColorId",
                table: "OrderItem");
        }
    }
}

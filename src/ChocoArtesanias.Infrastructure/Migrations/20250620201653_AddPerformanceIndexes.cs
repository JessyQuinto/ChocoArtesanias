using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChocoArtesanias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                newName: "IX_Order_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId_Featured",
                table: "Products",
                columns: new[] { "CategoryId", "Featured" });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Featured",
                table: "Products",
                column: "Featured");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Stock",
                table: "Products",
                column: "Stock");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreatedAt",
                table: "Orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Status",
                table: "Orders",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId_Featured",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Product_Featured",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Product_Stock",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Order_CreatedAt",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Order_Status",
                table: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }
    }
}

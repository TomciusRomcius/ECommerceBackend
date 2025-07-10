using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Foreign_Keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductStoreLocations_StoreLocationId",
                table: "ProductStoreLocations",
                column: "StoreLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_ProductId",
                table: "CartProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_StoreLocationId",
                table: "CartProducts",
                column: "StoreLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_AspNetUsers_UserId",
                table: "CartProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_Products_ProductId",
                table: "CartProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_StoreLocations_StoreLocationId",
                table: "CartProducts",
                column: "StoreLocationId",
                principalTable: "StoreLocations",
                principalColumn: "StoreLocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStoreLocations_Products_ProductId",
                table: "ProductStoreLocations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStoreLocations_StoreLocations_StoreLocationId",
                table: "ProductStoreLocations",
                column: "StoreLocationId",
                principalTable: "StoreLocations",
                principalColumn: "StoreLocationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_AspNetUsers_UserId",
                table: "CartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_Products_ProductId",
                table: "CartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_StoreLocations_StoreLocationId",
                table: "CartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStoreLocations_Products_ProductId",
                table: "ProductStoreLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStoreLocations_StoreLocations_StoreLocationId",
                table: "ProductStoreLocations");

            migrationBuilder.DropIndex(
                name: "IX_ProductStoreLocations_StoreLocationId",
                table: "ProductStoreLocations");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_ProductId",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_StoreLocationId",
                table: "CartProducts");
        }
    }
}

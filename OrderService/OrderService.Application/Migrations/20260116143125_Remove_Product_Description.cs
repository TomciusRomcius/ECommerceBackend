using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Application.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Product_Description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveOrders",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveOrders", x => new { x.UserId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_ActiveOrders_Orders_UserId",
                        column: x => x.UserId,
                        principalTable: "Orders",
                        principalColumn: "OrderEntityId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDeliveryAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCountInStockFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CountInStock",
                table: "foods",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountInStock",
                table: "foods");
        }
    }
}

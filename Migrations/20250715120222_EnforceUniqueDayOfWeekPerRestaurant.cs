using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sufra.Migrations
{
    /// <inheritdoc />
    public partial class EnforceUniqueDayOfWeekPerRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RestaurantOpeningHours_RestaurantId",
                table: "RestaurantOpeningHours");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantOpeningHours_RestaurantId_DayOfWeek",
                table: "RestaurantOpeningHours",
                columns: new[] { "RestaurantId", "DayOfWeek" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RestaurantOpeningHours_RestaurantId_DayOfWeek",
                table: "RestaurantOpeningHours");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantOpeningHours_RestaurantId",
                table: "RestaurantOpeningHours",
                column: "RestaurantId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sufra.Migrations
{
    /// <inheritdoc />
    public partial class AddCuisineImageToCuisine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CuisineImage",
                table: "Cuisines",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuisineImage",
                table: "Cuisines");
        }
    }
}

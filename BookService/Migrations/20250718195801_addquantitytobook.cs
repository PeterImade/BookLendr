using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.Migrations
{
    /// <inheritdoc />
    public partial class addquantitytobook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Books");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

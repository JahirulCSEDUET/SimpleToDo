using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleToDo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustQueryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Queries");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "Queries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "Queries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "Queries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleToDo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatorInfoModifiedAtTodoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Todos",
                newName: "CreatorId");

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Todos",
                newName: "CreatedBy");
        }
    }
}

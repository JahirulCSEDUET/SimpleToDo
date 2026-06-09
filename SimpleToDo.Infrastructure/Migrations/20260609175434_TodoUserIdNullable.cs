using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleToDo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TodoUserIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Members_UserId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Todos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Members_UserId",
                table: "Todos",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Members_UserId",
                table: "Todos");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Members_UserId",
                table: "Todos",
                column: "UserId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

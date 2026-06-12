using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleToDo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NotificationRedirectIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedirectId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectId",
                table: "Notifications");
        }
    }
}

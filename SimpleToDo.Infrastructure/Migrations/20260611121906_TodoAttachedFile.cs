using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleToDo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TodoAttachedFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Todos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Todos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Todos");
        }
    }
}

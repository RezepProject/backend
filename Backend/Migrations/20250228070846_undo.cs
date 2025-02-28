using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class undo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "process_personal_data",
                table: "usersession",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "reservation_url",
                table: "usersession",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 8, 46, 396, DateTimeKind.Utc).AddTicks(4765), new DateTime(2025, 3, 7, 7, 8, 46, 396, DateTimeKind.Utc).AddTicks(4773) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "process_personal_data",
                table: "usersession");

            migrationBuilder.DropColumn(
                name: "reservation_url",
                table: "usersession");

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 2, 27, 14, 15, 25, 299, DateTimeKind.Utc).AddTicks(9156), new DateTime(2025, 3, 6, 14, 15, 25, 299, DateTimeKind.Utc).AddTicks(9169) });
        }
    }
}

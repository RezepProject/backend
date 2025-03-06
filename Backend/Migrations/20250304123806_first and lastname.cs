using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class firstandlastname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "usersession",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "usersession",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "reservation_end",
                table: "usersession",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "reservation_start",
                table: "usersession",
                type: "date",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 3, 4, 12, 38, 4, 949, DateTimeKind.Utc).AddTicks(5611), new DateTime(2025, 3, 11, 12, 38, 4, 949, DateTimeKind.Utc).AddTicks(5630) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "first_name",
                table: "usersession");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "usersession");

            migrationBuilder.DropColumn(
                name: "reservation_end",
                table: "usersession");

            migrationBuilder.DropColumn(
                name: "reservation_start",
                table: "usersession");

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 14, 54, 33, DateTimeKind.Utc).AddTicks(5917), new DateTime(2025, 3, 7, 7, 14, 54, 33, DateTimeKind.Utc).AddTicks(5927) });
        }
    }
}

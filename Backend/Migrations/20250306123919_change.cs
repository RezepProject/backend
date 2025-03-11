using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reservation_url",
                table: "usersession",
                newName: "reservation_id");

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 3, 6, 12, 39, 19, 7, DateTimeKind.Utc).AddTicks(6250), new DateTime(2025, 3, 13, 12, 39, 19, 7, DateTimeKind.Utc).AddTicks(6257) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reservation_id",
                table: "usersession",
                newName: "reservation_url");

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 3, 4, 12, 40, 17, 473, DateTimeKind.Utc).AddTicks(8731), new DateTime(2025, 3, 11, 12, 40, 17, 473, DateTimeKind.Utc).AddTicks(8745) });
        }
    }
}

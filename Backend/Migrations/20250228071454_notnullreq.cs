using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class notnullreq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "reservation_url",
                table: "usersession",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 14, 54, 33, DateTimeKind.Utc).AddTicks(5917), new DateTime(2025, 3, 7, 7, 14, 54, 33, DateTimeKind.Utc).AddTicks(5927) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "reservation_url",
                table: "usersession",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 8, 46, 396, DateTimeKind.Utc).AddTicks(4765), new DateTime(2025, 3, 7, 7, 8, 46, 396, DateTimeKind.Utc).AddTicks(4773) });
        }
    }
}

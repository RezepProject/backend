using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class notnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "chat_gpt_thread_id",
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
                values: new object[] { new DateTime(2025, 3, 4, 12, 40, 17, 473, DateTimeKind.Utc).AddTicks(8731), new DateTime(2025, 3, 11, 12, 40, 17, 473, DateTimeKind.Utc).AddTicks(8745) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "chat_gpt_thread_id",
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
                values: new object[] { new DateTime(2025, 3, 4, 12, 38, 4, 949, DateTimeKind.Utc).AddTicks(5611), new DateTime(2025, 3, 11, 12, 38, 4, 949, DateTimeKind.Utc).AddTicks(5630) });
        }
    }
}

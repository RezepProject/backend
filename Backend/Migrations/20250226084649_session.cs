using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class session : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usersession",
                columns: table => new
                {
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    chat_gpt_thread_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usersession", x => x.session_id);
                });

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 2, 26, 8, 46, 48, 323, DateTimeKind.Utc).AddTicks(3205), new DateTime(2025, 3, 5, 8, 46, 48, 323, DateTimeKind.Utc).AddTicks(3213) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usersession");

            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 1, 28, 6, 54, 30, 104, DateTimeKind.Utc).AddTicks(2279), new DateTime(2025, 2, 4, 6, 54, 30, 104, DateTimeKind.Utc).AddTicks(2296) });
        }
    }
}

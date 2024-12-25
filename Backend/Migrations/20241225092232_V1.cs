using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "ADMIN" });

            migrationBuilder.InsertData(
                table: "configuser",
                columns: new[] { "id", "email", "first_name", "last_name", "password", "refresh_token", "role_id", "token_created", "token_expires" },
                values: new object[] { 1, "test", "test", "test", "$2a$11$TxzkGMQgywQjBxMq9YcOoO66hQODh5zJzIg4npGPDzfpcefvKORD2", "refresh_token_value", 1, new DateTime(2024, 12, 25, 9, 22, 32, 448, DateTimeKind.Utc).AddTicks(9902), new DateTime(2025, 1, 1, 9, 22, 32, 448, DateTimeKind.Utc).AddTicks(9910) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "role",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}

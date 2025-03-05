using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 3, 5, 18, 17, 52, 943, DateTimeKind.Utc).AddTicks(2860), new DateTime(2025, 3, 12, 18, 17, 52, 943, DateTimeKind.Utc).AddTicks(2868) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2025, 3, 5, 10, 26, 44, 114, DateTimeKind.Utc).AddTicks(3877), new DateTime(2025, 3, 12, 10, 26, 44, 114, DateTimeKind.Utc).AddTicks(3884) });
        }
    }
}

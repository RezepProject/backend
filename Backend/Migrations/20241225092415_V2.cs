﻿using System;
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
                values: new object[] { new DateTime(2024, 12, 25, 9, 24, 14, 713, DateTimeKind.Utc).AddTicks(4090), new DateTime(2025, 1, 1, 9, 24, 14, 713, DateTimeKind.Utc).AddTicks(4097) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "configuser",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "token_created", "token_expires" },
                values: new object[] { new DateTime(2024, 12, 25, 9, 22, 32, 448, DateTimeKind.Utc).AddTicks(9902), new DateTime(2025, 1, 1, 9, 22, 32, 448, DateTimeKind.Utc).AddTicks(9910) });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AIInUse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ai_in_use",
                table: "setting",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "setting",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "ai_in_use", "state" },
                values: new object[] { "ChatGPT", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ai_in_use",
                table: "setting");

            migrationBuilder.UpdateData(
                table: "setting",
                keyColumn: "id",
                keyValue: 1,
                column: "state",
                value: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class refreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_refresh_token",
                table: "refresh_token");

            migrationBuilder.RenameTable(
                name: "refresh_token",
                newName: "refreshtoken");

            migrationBuilder.AddPrimaryKey(
                name: "pk_refreshtoken",
                table: "refreshtoken",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_refreshtoken",
                table: "refreshtoken");

            migrationBuilder.RenameTable(
                name: "refreshtoken",
                newName: "refresh_token");

            migrationBuilder.AddPrimaryKey(
                name: "pk_refresh_token",
                table: "refresh_token",
                column: "id");
        }
    }
}

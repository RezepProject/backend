using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_config_user_roles_role_id",
                table: "ConfigUser");

            migrationBuilder.DropPrimaryKey(
                name: "pk_config_user",
                table: "ConfigUser");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "role");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "question");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "permission");

            migrationBuilder.RenameTable(
                name: "ConfigUser",
                newName: "configuser");

            migrationBuilder.RenameTable(
                name: "Config",
                newName: "config");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "answer");

            migrationBuilder.RenameIndex(
                name: "ix_config_user_role_id",
                table: "configuser",
                newName: "ix_configuser_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_configuser",
                table: "configuser",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_configuser_roles_role_id",
                table: "configuser",
                column: "role_id",
                principalTable: "role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_configuser_roles_role_id",
                table: "configuser");

            migrationBuilder.DropPrimaryKey(
                name: "pk_configuser",
                table: "configuser");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "question",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "permission",
                newName: "Permission");

            migrationBuilder.RenameTable(
                name: "configuser",
                newName: "ConfigUser");

            migrationBuilder.RenameTable(
                name: "config",
                newName: "Config");

            migrationBuilder.RenameTable(
                name: "answer",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "ix_configuser_role_id",
                table: "ConfigUser",
                newName: "ix_config_user_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_config_user",
                table: "ConfigUser",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_config_user_roles_role_id",
                table: "ConfigUser",
                column: "role_id",
                principalTable: "Role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_answers_questions_question_id",
                table: "answers");

            migrationBuilder.DropForeignKey(
                name: "fk_config_users_roles_role_id",
                table: "config_users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_roles",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_questions",
                table: "questions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_permissions",
                table: "permissions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_configs",
                table: "configs");

            migrationBuilder.DropPrimaryKey(
                name: "pk_config_users",
                table: "config_users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_answers",
                table: "answers");

            migrationBuilder.RenameTable(
                name: "roles",
                newName: "Role");

            migrationBuilder.RenameTable(
                name: "questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "permissions",
                newName: "Permission");

            migrationBuilder.RenameTable(
                name: "configs",
                newName: "Config");

            migrationBuilder.RenameTable(
                name: "config_users",
                newName: "ConfigUser");

            migrationBuilder.RenameTable(
                name: "answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "ix_config_users_role_id",
                table: "ConfigUser",
                newName: "ix_config_user_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_answers_question_id",
                table: "Answer",
                newName: "ix_answer_question_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_role",
                table: "Role",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_question",
                table: "Question",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_permission",
                table: "Permission",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_config",
                table: "Config",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_config_user",
                table: "ConfigUser",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_answer",
                table: "Answer",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_answer_questions_question_id",
                table: "Answer",
                column: "question_id",
                principalTable: "Question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_config_user_roles_role_id",
                table: "ConfigUser",
                column: "role_id",
                principalTable: "Role",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_answer_questions_question_id",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "fk_config_user_roles_role_id",
                table: "ConfigUser");

            migrationBuilder.DropPrimaryKey(
                name: "pk_role",
                table: "Role");

            migrationBuilder.DropPrimaryKey(
                name: "pk_question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "pk_permission",
                table: "Permission");

            migrationBuilder.DropPrimaryKey(
                name: "pk_config_user",
                table: "ConfigUser");

            migrationBuilder.DropPrimaryKey(
                name: "pk_config",
                table: "Config");

            migrationBuilder.DropPrimaryKey(
                name: "pk_answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "roles");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "questions");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "permissions");

            migrationBuilder.RenameTable(
                name: "ConfigUser",
                newName: "config_users");

            migrationBuilder.RenameTable(
                name: "Config",
                newName: "configs");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "answers");

            migrationBuilder.RenameIndex(
                name: "ix_config_user_role_id",
                table: "config_users",
                newName: "ix_config_users_role_id");

            migrationBuilder.RenameIndex(
                name: "ix_answer_question_id",
                table: "answers",
                newName: "ix_answers_question_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_roles",
                table: "roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_questions",
                table: "questions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_permissions",
                table: "permissions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_config_users",
                table: "config_users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_configs",
                table: "configs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_answers",
                table: "answers",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_answers_questions_question_id",
                table: "answers",
                column: "question_id",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_config_users_roles_role_id",
                table: "config_users",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

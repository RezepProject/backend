using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "backgroundimage",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    base64image = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_backgroundimage", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "config",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "question",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_question", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questioncategory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_questioncategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refreshtoken",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refreshtoken", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    config_user_id = table.Column<int>(type: "integer", nullable: false),
                    config_user = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    background_image = table.Column<string>(type: "text", nullable: false),
                    background_image_id = table.Column<int>(type: "integer", nullable: false),
                    language = table.Column<string>(type: "text", nullable: false),
                    talking_speed = table.Column<double>(type: "double precision", nullable: false),
                    greeting_message = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<bool>(type: "boolean", nullable: false),
                    ai_in_use = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_setting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false),
                    done = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "answer",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false),
                    user = table.Column<string>(type: "text", nullable: false),
                    question_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_answer", x => x.id);
                    table.ForeignKey(
                        name: "fk_answer_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "question",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "questionquestioncategory (dictionary<string, object>)",
                columns: table => new
                {
                    categories_id = table.Column<int>(type: "integer", nullable: false),
                    questions_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_questionquestioncategory_dictionary_string_object", x => new { x.categories_id, x.questions_id });
                    table.ForeignKey(
                        name: "fk_questionquestioncategory_dictionary_string_object_quest",
                        column: x => x.categories_id,
                        principalTable: "questioncategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_questionquestioncategory_dictionary_string_object_quest1",
                        column: x => x.questions_id,
                        principalTable: "question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "configuser",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: false),
                    token_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    token_expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configuser", x => x.id);
                    table.ForeignKey(
                        name: "fk_configuser_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "configusertoken",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    token = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configusertoken", x => x.id);
                    table.ForeignKey(
                        name: "fk_configusertoken_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "name" },
                values: new object[] { 1, "ADMIN" });

            migrationBuilder.InsertData(
                table: "setting",
                columns: new[] { "id", "ai_in_use", "background_image", "background_image_id", "config_user", "config_user_id", "greeting_message", "language", "name", "state", "talking_speed" },
                values: new object[] { 1, "ChatGPT", "https://example.com/image.jpg", 1, 0, 0, "Hello, how can I help you?", "en-US", "Rezep-1", true, 0.69999999999999996 });

            migrationBuilder.InsertData(
                table: "configuser",
                columns: new[] { "id", "email", "first_name", "last_name", "password", "refresh_token", "role_id", "token_created", "token_expires" },
                values: new object[] { 1, "test", "test", "test", "$2a$11$TxzkGMQgywQjBxMq9YcOoO66hQODh5zJzIg4npGPDzfpcefvKORD2", "refresh_token_value", 1, new DateTime(2025, 2, 21, 12, 56, 19, 335, DateTimeKind.Utc).AddTicks(9407), new DateTime(2025, 2, 28, 12, 56, 19, 335, DateTimeKind.Utc).AddTicks(9414) });

            migrationBuilder.CreateIndex(
                name: "ix_answer_question_id",
                table: "answer",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "ix_configuser_role_id",
                table: "configuser",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_configusertoken_role_id",
                table: "configusertoken",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_questionquestioncategory_dictionary_string_object_quest",
                table: "questionquestioncategory (dictionary<string, object>)",
                column: "questions_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answer");

            migrationBuilder.DropTable(
                name: "backgroundimage");

            migrationBuilder.DropTable(
                name: "config");

            migrationBuilder.DropTable(
                name: "configuser");

            migrationBuilder.DropTable(
                name: "configusertoken");

            migrationBuilder.DropTable(
                name: "questionquestioncategory (dictionary<string, object>)");

            migrationBuilder.DropTable(
                name: "refreshtoken");

            migrationBuilder.DropTable(
                name: "setting");

            migrationBuilder.DropTable(
                name: "task");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "questioncategory");

            migrationBuilder.DropTable(
                name: "question");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyQuestionCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_questioncategory_question_question_id",
                table: "questioncategory");

            migrationBuilder.DropIndex(
                name: "ix_questioncategory_question_id",
                table: "questioncategory");

            migrationBuilder.DropColumn(
                name: "question_id",
                table: "questioncategory");

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

            migrationBuilder.CreateIndex(
                name: "ix_questionquestioncategory_dictionary_string_object_quest",
                table: "questionquestioncategory (dictionary<string, object>)",
                column: "questions_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "questionquestioncategory (dictionary<string, object>)");

            migrationBuilder.AddColumn<int>(
                name: "question_id",
                table: "questioncategory",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_questioncategory_question_id",
                table: "questioncategory",
                column: "question_id");

            migrationBuilder.AddForeignKey(
                name: "fk_questioncategory_question_question_id",
                table: "questioncategory",
                column: "question_id",
                principalTable: "question",
                principalColumn: "id");
        }
    }
}

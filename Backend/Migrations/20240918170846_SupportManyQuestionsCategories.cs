using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SupportManyQuestionsCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_question_question_categories_category_id",
                table: "question");

            migrationBuilder.DropIndex(
                name: "ix_question_category_id",
                table: "question");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "question");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "question",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_question_category_id",
                table: "question",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "fk_question_question_categories_category_id",
                table: "question",
                column: "category_id",
                principalTable: "questioncategory",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

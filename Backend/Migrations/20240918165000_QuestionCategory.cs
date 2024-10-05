using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class QuestionCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "question",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_question_question_categories_category_id",
                table: "question");

            migrationBuilder.DropTable(
                name: "questioncategory");

            migrationBuilder.DropIndex(
                name: "ix_question_category_id",
                table: "question");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "question");
        }
    }
}

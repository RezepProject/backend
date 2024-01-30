using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_answer_questions_question_id",
                table: "answer");

            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                table: "configuser",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "token_created",
                table: "configuser",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "token_expires",
                table: "configuser",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "question_id",
                table: "answer",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "refresh_token",
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
                    table.PrimaryKey("pk_refresh_token", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_answer_questions_question_id",
                table: "answer",
                column: "question_id",
                principalTable: "question",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_answer_questions_question_id",
                table: "answer");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropColumn(
                name: "refresh_token",
                table: "configuser");

            migrationBuilder.DropColumn(
                name: "token_created",
                table: "configuser");

            migrationBuilder.DropColumn(
                name: "token_expires",
                table: "configuser");

            migrationBuilder.AlterColumn<int>(
                name: "question_id",
                table: "answer",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_answer_questions_question_id",
                table: "answer",
                column: "question_id",
                principalTable: "question",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

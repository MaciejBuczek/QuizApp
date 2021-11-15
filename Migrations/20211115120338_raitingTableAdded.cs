using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizApp.Migrations
{
    public partial class raitingTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Quizzes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<int>(nullable: false),
                    IdQuiz = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_Quizzes_IdQuiz",
                        column: x => x.IdQuiz,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_UserId",
                table: "Quizzes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_IdQuiz",
                table: "Rating",
                column: "IdQuiz");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_UserId",
                table: "Quizzes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_UserId",
                table: "Quizzes");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_UserId",
                table: "Quizzes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizApp.Migrations
{
    public partial class rating_userFK_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RatedBy",
                table: "Ratings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Ratings",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_RatedBy",
                table: "Ratings",
                column: "RatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedBy",
                table: "Ratings",
                column: "RatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_RatedBy",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_RatedBy",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "RatedBy",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ratings");
        }
    }
}

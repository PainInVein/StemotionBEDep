using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ThanhDT_Add_Table_Game : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    game_code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    lesson_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    config_data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    thumbnail_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.game_id);
                    table.ForeignKey(
                        name: "FK_Game.lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "Lesson",
                        principalColumn: "lesson_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameResult",
                columns: table => new
                {
                    game_result_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    game_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    correct_answers = table.Column<int>(type: "int", nullable: false),
                    total_questions = table.Column<int>(type: "int", nullable: false),
                    play_duration = table.Column<int>(type: "int", nullable: false),
                    played_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResult", x => x.game_result_id);
                    table.ForeignKey(
                        name: "FK_GameResult.game_id",
                        column: x => x.game_id,
                        principalTable: "Game",
                        principalColumn: "game_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameResult.student_id",
                        column: x => x.student_id,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_GameCode",
                table: "Game",
                column: "game_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_lesson_id",
                table: "Game",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_GameResult_game_id",
                table: "GameResult",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_GameResult_PlayedAt",
                table: "GameResult",
                column: "played_at");

            migrationBuilder.CreateIndex(
                name: "IX_GameResult_StudentId_GameId",
                table: "GameResult",
                columns: new[] { "student_id", "game_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameResult");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}

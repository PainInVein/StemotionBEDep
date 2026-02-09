using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ThanhDT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgress_student_id",
                table: "StudentProgress");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "StudentProgress",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProgress_UserId",
                table: "StudentProgress",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgress_User_UserId",
                table: "StudentProgress",
                column: "UserId",
                principalTable: "User",
                principalColumn: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgress_student_id",
                table: "StudentProgress",
                column: "student_id",
                principalTable: "Student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgress_User_UserId",
                table: "StudentProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentProgress_student_id",
                table: "StudentProgress");

            migrationBuilder.DropIndex(
                name: "IX_StudentProgress_UserId",
                table: "StudentProgress");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudentProgress");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentProgress_student_id",
                table: "StudentProgress",
                column: "student_id",
                principalTable: "User",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

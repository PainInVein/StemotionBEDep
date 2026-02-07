using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    student_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    parent_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    avatar_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    grade_level = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_Student_parent_id",
                        column: x => x.parent_id,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_parent_id",
                table: "Student",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Username",
                table: "Student",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}

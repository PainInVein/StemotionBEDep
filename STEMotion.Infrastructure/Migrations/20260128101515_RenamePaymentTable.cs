using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_payment_user_id",
                table: "payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payment",
                table: "payment");

            migrationBuilder.RenameTable(
                name: "payment",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_payment_user_id",
                table: "Payment",
                newName: "IX_Payment_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "payment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_UserId",
                table: "Payment",
                column: "user_id",
                principalTable: "User",
                principalColumn: "userId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_UserId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "payment");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_user_id",
                table: "payment",
                newName: "IX_payment_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payment",
                table: "payment",
                column: "payment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_payment_user_id",
                table: "payment",
                column: "user_id",
                principalTable: "User",
                principalColumn: "userId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

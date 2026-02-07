using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_userId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "payment");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "payment",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "transactionId",
                table: "payment",
                newName: "transaction_id");

            migrationBuilder.RenameColumn(
                name: "paymentDate",
                table: "payment",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "paymentId",
                table: "payment",
                newName: "payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_userId",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Payment",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "Payment",
                newName: "transactionId");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "Payment",
                newName: "paymentDate");

            migrationBuilder.RenameColumn(
                name: "payment_id",
                table: "Payment",
                newName: "paymentId");

            migrationBuilder.RenameIndex(
                name: "IX_payment_user_id",
                table: "Payment",
                newName: "IX_Payment_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "paymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_userId",
                table: "Payment",
                column: "userId",
                principalTable: "User",
                principalColumn: "userId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMotion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationShipPaymentSubscriptionPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubscriptionPayment_payment_id",
                table: "SubscriptionPayment");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayment_payment_id",
                table: "SubscriptionPayment",
                column: "payment_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubscriptionPayment_payment_id",
                table: "SubscriptionPayment");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayment_payment_id",
                table: "SubscriptionPayment",
                column: "payment_id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Subscriptions_SubscriptionId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Subscriptions_SubscriptionId",
                table: "Requests",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Subscriptions_SubscriptionId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Subscriptions_SubscriptionId",
                table: "Requests",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}

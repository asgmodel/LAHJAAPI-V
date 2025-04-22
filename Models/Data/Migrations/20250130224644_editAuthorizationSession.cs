using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editAuthorizationSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelGatewatId",
                table: "AuthorizationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModelGatewayId",
                table: "AuthorizationSessions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceId",
                table: "AuthorizationSessions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationSessions_ModelGatewayId",
                table: "AuthorizationSessions",
                column: "ModelGatewayId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationSessions_ServiceId",
                table: "AuthorizationSessions",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationSessions_ModelGateways_ModelGatewayId",
                table: "AuthorizationSessions",
                column: "ModelGatewayId",
                principalTable: "ModelGateways",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationSessions_Services_ServiceId",
                table: "AuthorizationSessions",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationSessions_ModelGateways_ModelGatewayId",
                table: "AuthorizationSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationSessions_Services_ServiceId",
                table: "AuthorizationSessions");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationSessions_ModelGatewayId",
                table: "AuthorizationSessions");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationSessions_ServiceId",
                table: "AuthorizationSessions");

            migrationBuilder.DropColumn(
                name: "ModelGatewatId",
                table: "AuthorizationSessions");

            migrationBuilder.DropColumn(
                name: "ModelGatewayId",
                table: "AuthorizationSessions");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "AuthorizationSessions");
        }
    }
}

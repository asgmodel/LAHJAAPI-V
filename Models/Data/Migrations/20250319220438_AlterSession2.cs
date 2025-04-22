using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterSession2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationSessions_ModelAis_ModelAiId",
                table: "AuthorizationSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationSessions_ModelGateways_ModelGatewayId",
                table: "AuthorizationSessions");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationSessions_ModelAiId",
                table: "AuthorizationSessions");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationSessions_ModelGatewayId",
                table: "AuthorizationSessions");

            migrationBuilder.DropColumn(
                name: "ModelAiId",
                table: "AuthorizationSessions");

            migrationBuilder.DropColumn(
                name: "ModelGatewayId",
                table: "AuthorizationSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelAiId",
                table: "AuthorizationSessions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModelGatewayId",
                table: "AuthorizationSessions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationSessions_ModelAiId",
                table: "AuthorizationSessions",
                column: "ModelAiId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationSessions_ModelGatewayId",
                table: "AuthorizationSessions",
                column: "ModelGatewayId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationSessions_ModelAis_ModelAiId",
                table: "AuthorizationSessions",
                column: "ModelAiId",
                principalTable: "ModelAis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationSessions_ModelGateways_ModelGatewayId",
                table: "AuthorizationSessions",
                column: "ModelGatewayId",
                principalTable: "ModelGateways",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

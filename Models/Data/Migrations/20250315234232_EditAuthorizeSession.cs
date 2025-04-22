using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class EditAuthorizeSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationSessions_Services_ServiceId",
                table: "AuthorizationSessions");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationSessions_ServiceId",
                table: "AuthorizationSessions");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "AuthorizationSessions");

            migrationBuilder.AddColumn<string>(
                name: "ServicesId",
                table: "AuthorizationSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicesId",
                table: "AuthorizationSessions");

            migrationBuilder.AddColumn<string>(
                name: "ServiceId",
                table: "AuthorizationSessions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationSessions_ServiceId",
                table: "AuthorizationSessions",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationSessions_Services_ServiceId",
                table: "AuthorizationSessions",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addTableUserService2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserService_AspNetUsers_UserId",
                table: "UserService");

            migrationBuilder.DropForeignKey(
                name: "FK_UserService_Services_ServiceId",
                table: "UserService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserService",
                table: "UserService");

            migrationBuilder.RenameTable(
                name: "UserService",
                newName: "UserServices");

            migrationBuilder.RenameIndex(
                name: "IX_UserService_UserId",
                table: "UserServices",
                newName: "IX_UserServices_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserService_ServiceId",
                table: "UserServices",
                newName: "IX_UserServices_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserServices_AspNetUsers_UserId",
                table: "UserServices",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserServices_Services_ServiceId",
                table: "UserServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserServices_AspNetUsers_UserId",
                table: "UserServices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserServices_Services_ServiceId",
                table: "UserServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices");

            migrationBuilder.RenameTable(
                name: "UserServices",
                newName: "UserService");

            migrationBuilder.RenameIndex(
                name: "IX_UserServices_UserId",
                table: "UserService",
                newName: "IX_UserService_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserServices_ServiceId",
                table: "UserService",
                newName: "IX_UserService_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserService",
                table: "UserService",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserService_AspNetUsers_UserId",
                table: "UserService",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserService_Services_ServiceId",
                table: "UserService",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

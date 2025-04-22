using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addTableUserModelAi2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices");

            migrationBuilder.DropIndex(
                name: "IX_UserServices_UserId",
                table: "UserServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModelAis",
                table: "UserModelAis");

            migrationBuilder.DropIndex(
                name: "IX_UserModelAis_UserId",
                table: "UserModelAis");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserServices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices",
                columns: new[] { "UserId", "ServiceId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModelAis",
                table: "UserModelAis",
                columns: new[] { "UserId", "ModelAiId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModelAis",
                table: "UserModelAis");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserServices",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModelAis",
                table: "UserModelAis",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserServices_UserId",
                table: "UserServices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModelAis_UserId",
                table: "UserModelAis",
                column: "UserId");
        }
    }
}

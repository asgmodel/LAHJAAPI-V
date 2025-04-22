using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addColumnToModelAi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelGatewayId",
                table: "ModelAis",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModelAis_ModelGatewayId",
                table: "ModelAis",
                column: "ModelGatewayId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelAis_ModelGateways_ModelGatewayId",
                table: "ModelAis",
                column: "ModelGatewayId",
                principalTable: "ModelGateways",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelAis_ModelGateways_ModelGatewayId",
                table: "ModelAis");

            migrationBuilder.DropIndex(
                name: "IX_ModelAis_ModelGatewayId",
                table: "ModelAis");

            migrationBuilder.DropColumn(
                name: "ModelGatewayId",
                table: "ModelAis");
        }
    }
}

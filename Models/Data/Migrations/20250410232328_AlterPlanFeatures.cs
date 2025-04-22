using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterPlanFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "PlanFeatures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "PlanFeatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeatures_Key",
                table: "PlanFeatures",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlanFeatures_Key",
                table: "PlanFeatures");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "PlanFeatures");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PlanFeatures");
        }
    }
}

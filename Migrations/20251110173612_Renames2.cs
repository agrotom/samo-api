using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samo_api.Migrations
{
    /// <inheritdoc />
    public partial class Renames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insights_users_UserId",
                table: "Insights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Insights",
                table: "Insights");

            migrationBuilder.RenameTable(
                name: "Insights",
                newName: "insights");

            migrationBuilder.RenameIndex(
                name: "IX_Insights_UserId",
                table: "insights",
                newName: "IX_insights_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_insights",
                table: "insights",
                column: "Date");

            migrationBuilder.AddForeignKey(
                name: "FK_insights_users_UserId",
                table: "insights",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_insights_users_UserId",
                table: "insights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_insights",
                table: "insights");

            migrationBuilder.RenameTable(
                name: "insights",
                newName: "Insights");

            migrationBuilder.RenameIndex(
                name: "IX_insights_UserId",
                table: "Insights",
                newName: "IX_Insights_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Insights",
                table: "Insights",
                column: "Date");

            migrationBuilder.AddForeignKey(
                name: "FK_Insights_users_UserId",
                table: "Insights",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

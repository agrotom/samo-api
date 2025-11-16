using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samo_api.Migrations
{
    /// <inheritdoc />
    public partial class TextEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_resulst_users_UserId",
                table: "resulst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_resulst",
                table: "resulst");

            migrationBuilder.RenameTable(
                name: "resulst",
                newName: "results");

            migrationBuilder.RenameIndex(
                name: "IX_resulst_UserId",
                table: "results",
                newName: "IX_results_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_results",
                table: "results",
                column: "Date");

            migrationBuilder.AddForeignKey(
                name: "FK_results_users_UserId",
                table: "results",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_results_users_UserId",
                table: "results");

            migrationBuilder.DropPrimaryKey(
                name: "PK_results",
                table: "results");

            migrationBuilder.RenameTable(
                name: "results",
                newName: "resulst");

            migrationBuilder.RenameIndex(
                name: "IX_results_UserId",
                table: "resulst",
                newName: "IX_resulst_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_resulst",
                table: "resulst",
                column: "Date");

            migrationBuilder.AddForeignKey(
                name: "FK_resulst_users_UserId",
                table: "resulst",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

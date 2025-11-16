using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samo_api.Migrations
{
    /// <inheritdoc />
    public partial class TextEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "focuses",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_focuses", x => x.Date);
                    table.ForeignKey(
                        name: "FK_focuses_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "problems",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problems", x => x.Date);
                    table.ForeignKey(
                        name: "FK_problems_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "resulst",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resulst", x => x.Date);
                    table.ForeignKey(
                        name: "FK_resulst_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "thanks",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thanks", x => x.Date);
                    table.ForeignKey(
                        name: "FK_thanks_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tomorrowMainTasks",
                columns: table => new
                {
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tomorrowMainTasks", x => x.Date);
                    table.ForeignKey(
                        name: "FK_tomorrowMainTasks_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_focuses_UserId",
                table: "focuses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_problems_UserId",
                table: "problems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_resulst_UserId",
                table: "resulst",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_thanks_UserId",
                table: "thanks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tomorrowMainTasks_UserId",
                table: "tomorrowMainTasks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "focuses");

            migrationBuilder.DropTable(
                name: "problems");

            migrationBuilder.DropTable(
                name: "resulst");

            migrationBuilder.DropTable(
                name: "thanks");

            migrationBuilder.DropTable(
                name: "tomorrowMainTasks");
        }
    }
}

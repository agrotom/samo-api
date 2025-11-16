using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace samo_api.Migrations
{
    /// <inheritdoc />
    public partial class newModelsFix5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "week_habitat");

            migrationBuilder.CreateTable(
                name: "week_habit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Checks = table.Column<bool[]>(type: "boolean[]", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_week_habit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_week_habit_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_week_habit_UserId",
                table: "week_habit",
                column: "UserId");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION public.get_start_of_day(ts timestamp with time zone, tz text)
                RETURNS timestamp without time zone AS $$
                BEGIN
                RETURN DATE_TRUNC('day', ts AT TIME ZONE tz);
                END;
                $$ LANGUAGE plpgsql IMMUTABLE;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "week_habit");

            migrationBuilder.CreateTable(
                name: "week_habitat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Checks = table.Column<bool[]>(type: "boolean[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_week_habitat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_week_habitat_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_week_habitat_UserId",
                table: "week_habitat",
                column: "UserId");
        }
    }
}

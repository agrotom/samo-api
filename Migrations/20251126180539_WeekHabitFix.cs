using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samo_api.Migrations
{
    /// <inheritdoc />
    public partial class WeekHabitFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "week_habit",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "week_habit",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Week",
                table: "week_habit");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "week_habit");
        }
    }
}

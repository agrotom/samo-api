using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samo_api.Migrations
{
    /// <inheritdoc />
    public partial class newModelsFix3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "work_happiness",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "week_habitat",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "self_happiness",
                newName: "CreatedAt");

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
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "work_happiness",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "week_habitat",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "self_happiness",
                newName: "Date");
        }
    }
}

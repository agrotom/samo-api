using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace samo_api.Migrations
{
    /// <inheritdoc />
    public partial class newModelsFix4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        }
    }
}

#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class FixSearchVectorDepartmentValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION equipment.update_equipment_users_search_vector()
                  RETURNS TRIGGER AS
                $$
                BEGIN
                  NEW.""SearchVector"" :=
                    to_tsvector('simple', COALESCE(NEW.""FirstName"", '') || ' ' || COALESCE(NEW.""LastName"", '') || ' ' || COALESCE(NEW.""DepartmentId"", ''))::tsvector;
                  RETURN NEW;
                END;
                $$
                LANGUAGE plpgsql;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

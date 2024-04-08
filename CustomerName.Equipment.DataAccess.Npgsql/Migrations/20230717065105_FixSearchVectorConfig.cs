#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class FixSearchVectorConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION equipment.update_equipments_search_vector()
                RETURNS TRIGGER AS $$
                BEGIN
                  NEW.""SearchVector"" :=
                    to_tsvector('simple', NEW.""Name"") ||
                    to_tsvector('simple', NEW.""Type"") ||
                    to_tsvector('simple', NEW.""SerialNumber"") ||
                    to_tsvector('simple', NEW.""PurchasePlace"") ||
                    to_tsvector('simple', NEW.""Location"") ||
                    to_tsvector('simple', CAST(NEW.""Id"" AS VARCHAR))::tsvector;
                  RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;");

            migrationBuilder.Sql(@"UPDATE equipment.""Equipments"" SET ""Name"" = ""Name"";");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION equipment.update_equipment_users_search_vector()
                  RETURNS TRIGGER AS
                $$
                BEGIN
                  NEW.""SearchVector"" :=
                    to_tsvector('simple', COALESCE(NEW.""FirstName"", '') || ' ' || COALESCE(NEW.""LastName"", '') || ' ' || COALESCE(NEW.""Department"", ''))::tsvector;
                  RETURN NEW;
                END;
                $$
                LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"UPDATE equipment.""EquipmentUsers"" SET ""FirstName"" = ""FirstName"";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION equipment.update_equipments_search_vector()
                RETURNS TRIGGER AS $$
                BEGIN
                  NEW.""SearchVector"" :=
                    to_tsvector('pg_catalog.english', NEW.""Name"") ||
                    to_tsvector('pg_catalog.english', NEW.""Type"") ||
                    to_tsvector('pg_catalog.english', NEW.""SerialNumber"") ||
                    to_tsvector('pg_catalog.english', NEW.""PurchasePlace"") ||
                    to_tsvector('pg_catalog.english', NEW.""Location"") ||
                    to_tsvector('pg_catalog.english', CAST(NEW.""Id"" AS VARCHAR));
                  RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION equipment.update_equipment_users_search_vector()
                  RETURNS TRIGGER AS
                $$
                BEGIN
                  NEW.""SearchVector"" :=
                    to_tsvector('pg_catalog.english', COALESCE(NEW.""FirstName"", '') || ' ' || COALESCE(NEW.""LastName"", '') || ' ' || COALESCE(NEW.""Department"", ''));
                  RETURN NEW;
                END;
                $$
                LANGUAGE plpgsql;
            ");
        }
    }
}

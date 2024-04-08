using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSearchVectors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EquipmentUsers_SearchVector",
                schema: "equipment",
                table: "EquipmentUsers");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_SearchVector",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                schema: "equipment",
                table: "EquipmentUsers");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.Sql("""
                                 DROP TRIGGER IF EXISTS equipments_search_vector_update_trigger
                                 ON equipment."Equipments"
                                 """);

            migrationBuilder.Sql("""
                                 DROP TRIGGER IF EXISTS equipmentUsers_search_vector_update_trigger
                                 ON equipment."EquipmentUsers"
                                 """);

            migrationBuilder.Sql("""
                                 DROP FUNCTION IF EXISTS equipment.update_equipment_users_search_vector;
                                 """);

            migrationBuilder.Sql("""
                                 DROP FUNCTION IF EXISTS equipment.update_equipments_search_vector;
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "tsvector",
                nullable: true);

            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                schema: "equipment",
                table: "Equipments",
                type: "tsvector",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsers_SearchVector",
                schema: "equipment",
                table: "EquipmentUsers",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_SearchVector",
                schema: "equipment",
                table: "Equipments",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.Sql(@"
                CREATE TRIGGER equipments_search_vector_update_trigger
                BEFORE INSERT OR UPDATE
                ON equipment.""Equipments""
                FOR EACH ROW
                EXECUTE FUNCTION equipment.update_equipments_search_vector();
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER equipmentUsers_search_vector_update_trigger
                BEFORE INSERT OR UPDATE
                ON ""equipment"".""EquipmentUsers""
                FOR EACH ROW
                EXECUTE FUNCTION equipment.update_equipment_users_search_vector();
            ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION equipment.update_equipments_search_vector()
                RETURNS TRIGGER AS $$
                BEGIN
                  NEW.""SearchVector"" :=
                    to_tsvector('simple', NEW.""Name"") ||
                    to_tsvector('simple', NEW.""TypeId"") ||
                    to_tsvector('simple', NEW.""SerialNumber"") ||
                    to_tsvector('simple', NEW.""PurchasePlace"") ||
                    to_tsvector('simple', NEW.""Location"") ||
                    to_tsvector('simple', CAST(NEW.""Id"" AS VARCHAR))::tsvector;
                  RETURN NEW;
                END;
                $$ LANGUAGE plpgsql;");

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
    }
}

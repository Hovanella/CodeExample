#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class Equipment_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "equipment");

            migrationBuilder.CreateTable(
                name: "EquipmentUsers",
                schema: "equipment",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: true),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                schema: "equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PurchasePrice = table.Column<double>(type: "double precision", nullable: false),
                    PurchasePriceUsd = table.Column<double>(type: "double precision", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PurchasePlace = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    GuaranteeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Characteristics = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ApproverId = table.Column<int>(type: "integer", nullable: true),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalSchema: "equipment",
                        principalTable: "EquipmentUsers",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentAssigns",
                schema: "equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedToUserId = table.Column<int>(type: "integer", nullable: false),
                    EquipmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentAssigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentAssigns_EquipmentUsers_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalSchema: "equipment",
                        principalTable: "EquipmentUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentAssigns_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalSchema: "equipment",
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssigns_AssignedToUserId",
                schema: "equipment",
                table: "EquipmentAssigns",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssigns_EquipmentId",
                schema: "equipment",
                table: "EquipmentAssigns",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_ApproverId",
                schema: "equipment",
                table: "Equipments",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_SearchVector",
                schema: "equipment",
                table: "Equipments",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_SerialNumber",
                schema: "equipment",
                table: "Equipments",
                column: "SerialNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_Type",
                schema: "equipment",
                table: "Equipments",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsers_Email",
                schema: "equipment",
                table: "EquipmentUsers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsers_SearchVector",
                schema: "equipment",
                table: "EquipmentUsers",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

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
                CREATE TRIGGER equipments_search_vector_update_trigger
                BEFORE INSERT OR UPDATE
                ON equipment.""Equipments""
                FOR EACH ROW
                EXECUTE FUNCTION equipment.update_equipments_search_vector();
            ");

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

            migrationBuilder.Sql(@"
                CREATE TRIGGER equipmentUsers_search_vector_update_trigger
                BEFORE INSERT OR UPDATE
                ON ""equipment"".""EquipmentUsers""
                FOR EACH ROW
                EXECUTE FUNCTION equipment.update_equipment_users_search_vector();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentAssigns",
                schema: "equipment");

            migrationBuilder.DropTable(
                name: "Equipments",
                schema: "equipment");

            migrationBuilder.DropTable(
                name: "EquipmentUsers",
                schema: "equipment");
        }
    }
}

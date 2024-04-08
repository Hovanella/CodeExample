#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentReports",
                schema: "equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNumber = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Data = table.Column<string>(type: "json", nullable: false),
                    DataHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    AssembledAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentReportRelevancePeriods",
                schema: "equipment",
                columns: table => new
                {
                    FromUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EquipmentReportId = table.Column<int>(type: "integer", nullable: false),
                    ToUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentReportRelevancePeriods", x => new { x.EquipmentReportId, x.FromUtc });
                    table.ForeignKey(
                        name: "FK_EquipmentReportRelevancePeriods_EquipmentReports_EquipmentR~",
                        column: x => x.EquipmentReportId,
                        principalSchema: "equipment",
                        principalTable: "EquipmentReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentReportRelevancePeriods",
                schema: "equipment");

            migrationBuilder.DropTable(
                name: "EquipmentReports",
                schema: "equipment");
        }
    }
}

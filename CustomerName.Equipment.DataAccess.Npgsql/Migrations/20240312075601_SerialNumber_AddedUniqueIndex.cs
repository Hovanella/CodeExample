using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class SerialNumber_AddedUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Equipments_SerialNumber",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_SerialNumber",
                schema: "equipment",
                table: "Equipments",
                column: "SerialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Equipments_SerialNumber",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_SerialNumber",
                schema: "equipment",
                table: "Equipments",
                column: "SerialNumber");
        }
    }
}

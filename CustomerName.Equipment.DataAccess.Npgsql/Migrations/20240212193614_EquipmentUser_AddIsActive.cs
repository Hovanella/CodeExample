using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsers_IsActive",
                schema: "equipment",
                table: "EquipmentUsers",
                column: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EquipmentUsers_IsActive",
                schema: "equipment",
                table: "EquipmentUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "equipment",
                table: "EquipmentUsers");
        }
    }
}

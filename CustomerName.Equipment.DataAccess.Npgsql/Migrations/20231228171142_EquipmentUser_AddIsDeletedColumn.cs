#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentUser_AddIsDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsers_IsDeleted",
                schema: "equipment",
                table: "EquipmentUsers",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EquipmentUsers_IsDeleted",
                schema: "equipment",
                table: "EquipmentUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "equipment",
                table: "EquipmentUsers");
        }
    }
}

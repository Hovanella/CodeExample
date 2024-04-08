using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentUser_RemoveIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 UPDATE equipment."EquipmentUsers" SET "IsActive" = NOT "IsDeleted"
                                 """);

            migrationBuilder.DropIndex(
                name: "IX_EquipmentUsers_IsDeleted",
                schema: "equipment",
                table: "EquipmentUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "equipment",
                table: "EquipmentUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.Sql("""
                                 UPDATE equipment."EquipmentUsers" SET "IsDeleted" = NOT "IsActive"
                                 """);
        }
    }
}

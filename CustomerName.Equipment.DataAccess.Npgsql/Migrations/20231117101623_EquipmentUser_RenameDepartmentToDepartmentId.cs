#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentUser_RenameDepartmentToDepartmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Department",
                schema: "equipment",
                table: "EquipmentUsers",
                newName: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                schema: "equipment",
                table: "EquipmentUsers",
                newName: "Department");
        }
    }
}

#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeOfId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewIntId",
                schema: "equipment",
                table: "Equipments",
                type: "integer",
                nullable: false,
                defaultValueSql: "ABS((floor(random() * 100000))::int)");

            migrationBuilder.AddColumn<int>(
                name: "NewEquipmentId",
                schema: "equipment",
                table: "EquipmentAssigns",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NewIntId",
                schema: "equipment",
                table: "EquipmentAssigns",
                type: "integer",
                nullable: false,
                defaultValueSql: "ABS((floor(random() * 100000))::int)");

            migrationBuilder.Sql(@"
                UPDATE equipment.""EquipmentAssigns""
                SET ""NewEquipmentId"" = (SELECT ""NewIntId"" FROM equipment.""Equipments"" WHERE equipment.""EquipmentAssigns"".""EquipmentId"" = ""Id"")
            ");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                schema: "equipment",
                table: "EquipmentAssigns");

            migrationBuilder.RenameColumn(
                name: "NewEquipmentId",
                schema: "equipment",
                table: "EquipmentAssigns",
                newName: "EquipmentId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.RenameColumn(
                name: "NewIntId",
                schema: "equipment",
                table: "Equipments",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                schema: "equipment",
                table: "Equipments",
                column: "Id");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentAssigns",
                schema: "equipment",
                table: "EquipmentAssigns");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "equipment",
                table: "EquipmentAssigns");

            migrationBuilder.RenameColumn(
                name: "NewIntId",
                schema: "equipment",
                table: "EquipmentAssigns",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentAssigns",
                schema: "equipment",
                table: "EquipmentAssigns",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NewGuidId",
                schema: "equipment",
                table: "Equipments",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "NewEquipmentId",
                schema: "equipment",
                table: "EquipmentAssigns",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "NewGuidId",
                schema: "equipment",
                table: "EquipmentAssigns",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()");

            migrationBuilder.Sql(@"
                UPDATE equipment.""EquipmentAssigns""
                SET ""NewEquipmentId"" = (SELECT ""NewGuidId"" FROM equipment.""Equipments"" WHERE equipment.""EquipmentAssigns"".""EquipmentId"" = ""Id"")
            ");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                schema: "equipment",
                table: "EquipmentAssigns");

            migrationBuilder.RenameColumn(
                name: "NewEquipmentId",
                schema: "equipment",
                table: "EquipmentAssigns",
                newName: "EquipmentId");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.RenameColumn(
                name: "NewGuidId",
                schema: "equipment",
                table: "Equipments",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                schema: "equipment",
                table: "Equipments",
                column: "Id");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EquipmentAssigns",
                schema: "equipment",
                table: "EquipmentAssigns");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "equipment",
                table: "EquipmentAssigns");

            migrationBuilder.RenameColumn(
                name: "NewGuidId",
                schema: "equipment",
                table: "EquipmentAssigns",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EquipmentAssigns",
                schema: "equipment",
                table: "EquipmentAssigns",
                column: "Id");
        }
    }
}

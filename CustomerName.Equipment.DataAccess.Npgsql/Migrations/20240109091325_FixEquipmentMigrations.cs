#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class FixEquipmentMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                schema: "equipment",
                table: "Equipments",
                type: "character varying(32)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                schema: "equipment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedById = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.Sql(@$"
                INSERT INTO  equipment.""EquipmentTypes""(""Id"", ""ShortName"", ""FullName"", ""CreatedById"", ""CreatedAtUtc"" )
                VALUES
                    ('DesktopComputer','DesktopComputer','Desktop computer',{PortalConstants.SystemUserId},now() at time zone 'utc'),
                    ('Laptop','Laptop','Laptop',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('MacBook','MacBook','MacBook',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('MacBookMini','MacBookMini','MacBook mini',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('MobilePhone','MobilePhone','Mobile phone',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Monitor','Monitor','Monitor',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Keyboard','Keyboard','Keyboard',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Mouse', 'Mouse', 'Mouse',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('MouseWithKeyboard','MouseWithKeyboard', 'Mouse + keyboard',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Headset','Headset','Headset',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('WebCamera','WebCamera','Web camera',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Firewall','Firewall','Firewall',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('AccessPoint','AccessPoint','Access point',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Switch','Switch','Switch',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Router','Router','Router',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('NetworkAdapter','NetworkAdapter','Network adapter',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('Printer','Printer','Printer',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('FlashDrive','FlashDrive','Flash drive',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('ExternalHardDrive','ExternalHardDrive', 'External hard drive',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('MultiAdapter','MultiAdapter','Multi-adapter',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('PcAccessoriesHdd','PcAccessoriesHdd','PC accessory: HDD',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('PcAccessoriesSsd','PcAccessoriesSsd','PC accessory: SSD',{PortalConstants.SystemUserId} , now() at time zone 'utc'),
                    ('PcAccessoriesRam','PcAccessoriesRam','PC accessory: RAM',{PortalConstants.SystemUserId} , now() at time zone 'utc')
            ");

            migrationBuilder.Sql(@"UPDATE equipment.""Equipments""
                SET ""TypeId"" = equipment.""EquipmentTypes"".""Id""
                FROM equipment.""EquipmentTypes""
                WHERE (equipment.""Equipments"".""Type"" = equipment.""EquipmentTypes"".""FullName"" OR equipment.""Equipments"".""Type"" = equipment.""EquipmentTypes"".""Id"");");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_TypeId",
                schema: "equipment",
                table: "Equipments",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_EquipmentTypes_TypeId",
                schema: "equipment",
                table: "Equipments",
                column: "TypeId",
                principalSchema: "equipment",
                principalTable: "EquipmentTypes",
                principalColumn: "Id");

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

            migrationBuilder.DropIndex(
                name: "IX_Equipments_Type",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "equipment",
                table: "Equipments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_EquipmentTypes_TypeId",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.DropTable(
                name: "EquipmentTypes",
                schema: "equipment");

            migrationBuilder.DropIndex(
                name: "IX_Equipments_TypeId",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "TypeId",
                schema: "equipment",
                table: "Equipments");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                schema: "equipment",
                table: "Equipments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_Type",
                schema: "equipment",
                table: "Equipments",
                column: "Type");
        }
    }
}

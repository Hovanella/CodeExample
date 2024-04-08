#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEquipmentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE equipment.""Equipments""
                SET ""Type"" = CASE
                    WHEN ""Type"" = 'Access Point' THEN 'Access point'
                    WHEN ""Type"" = 'Desktop Computer' THEN 'Desktop computer'
                    WHEN ""Type"" = 'Mouse and Keyboard' THEN 'Mouse + keyboard'
                    WHEN ""Type"" = 'Network Adapter' THEN 'Network adapter'
                    WHEN ""Type"" = 'Flash Drive' THEN 'Flash drive'
                    WHEN ""Type"" = 'External Hard Drive' THEN 'External hard drive'
                    WHEN ""Type"" = 'Multi-Adapter' THEN 'Multi-adapter'
                    WHEN ""Type"" = 'PC Accessority: HDD' THEN 'PC accessory: HDD'
                    WHEN ""Type"" = 'PC Accessority: SSD' THEN 'PC accessory: SSD'
                    WHEN ""Type"" = 'PC Accessority: RAM' THEN 'PC accessory: RAM'
                    WHEN ""Type"" = 'External Hard Drive' THEN 'External hard drive'
                    WHEN ""Type"" = 'Mobile Phone' THEN 'Mobile phone'
                    WHEN ""Type"" = 'Web Camera' THEN 'Web camera'
                    WHEN ""Type"" = 'MacBook Mini' THEN 'MacBook mini'
                    ELSE ""Type""
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE equipment.""Equipments""
                SET ""Type"" = CASE
                    WHEN ""Type"" = 'Access point' THEN 'Access Point'
                    WHEN ""Type"" = 'Desktop computer' THEN 'Desktop Computer'
                    WHEN ""Type"" = 'Mouse and keyboard' THEN 'Mouse + Keyboard'
                    WHEN ""Type"" = 'Network adapter' THEN 'Network Adapter'
                    WHEN ""Type"" = 'Flash drive' THEN 'Flash Drive'
                    WHEN ""Type"" = 'External hard drive' THEN 'External Hard Drive'
                    WHEN ""Type"" = 'Multi-adapter' THEN 'Multi-Adapter'
                    WHEN ""Type"" = 'PC accessory: HDD' THEN 'PC Accessority: HDD'
                    WHEN ""Type"" = 'PC accessory: SSD' THEN 'PC Accessority: SSD'
                    WHEN ""Type"" = 'PC accessory: RAM' THEN 'PC Accessority: RAM'
                    WHEN ""Type"" = 'External hard drive' THEN 'External Hard Drive'
                    WHEN ""Type"" = 'Mobile phone' THEN 'Mobile Phone'
                    WHEN ""Type"" = 'Web camera' THEN 'Web Camera'
                    WHEN ""Type"" = 'MacBook mini' THEN 'MacBook Mini'
                    ELSE ""Type""
                END;
            ");
        }
    }
}

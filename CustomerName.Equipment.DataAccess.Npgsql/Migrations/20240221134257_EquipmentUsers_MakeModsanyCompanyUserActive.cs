using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentUsers_MakeModsanyCompanyUserActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 update equipment."EquipmentUsers"
                                 set "IsActive"=true
                                 where "Email"='CustomerNameCompany@CustomerName-software.com' and "IsActive" = false
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 update equipment."EquipmentUsers"
                                 set "IsActive"=false
                                 where "Email"='CustomerNameCompany@CustomerName-software.com' and "IsActive" = true
                                 """);
        }
    }
}

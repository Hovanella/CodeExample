using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class MaxLengthsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    DROP VIEW equipment."EquipmentOdataView";
                                 """);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "equipment",
                table: "EquipmentTypes",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "TypeId",
                schema: "equipment",
                table: "Equipments",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Characteristics",
                schema: "equipment",
                table: "Equipments",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.Sql("""
                                 CREATE OR REPLACE VIEW equipment."EquipmentOdataView" AS
                                 SELECT E."Id",
                                        "Name",
                                        "Location",
                                        "SerialNumber",
                                        "PurchasePrice",
                                        "PurchaseCurrency",
                                        "PurchasePriceUsd",
                                        "PurchaseDate",
                                        "PurchasePlace",
                                        "GuaranteeDate",
                                        "Characteristics",
                                        "Comment",
                                        "ApproverId",
                                        concat_ws(' ',EU."LastName", EU."FirstName") as "ApproverFullName",
                                        ET."Id"        as "EquipmentTypeId",
                                        ET."ShortName" as "EquipmentTypeShortName",
                                        ET."FullName"  as "EquipmentTypeFullName",
                                        AH."ActiveHolderId",
                                        concat_ws(' ',AH."ActiveHolderLastName",AH."ActiveHolderFirstName") as "ActiveHolderFullName",
                                        AH."ActiveHolderDepartmentId",
                                        AH."ActiveHolderIssueDate",
                                        AH."ActiveHolderReturnDate",
                                        "InvoiceNumber"

                                 FROM equipment."Equipments" E
                                          LEFT JOIN equipment."EquipmentUsers" EU ON E."ApproverId" = EU."UserId"
                                          JOIN equipment."EquipmentTypes" ET on ET."Id" = E."TypeId"
                                          LEFT JOIN (

                                     SELECT EA."EquipmentId" as "ActiveHolderEquipmentId",
                                            EU."UserId"           as "ActiveHolderId",
                                            EU."FirstName"    as "ActiveHolderFirstName",
                                            EU."LastName"     as "ActiveHolderLastName",
                                            EU."DepartmentId" as "ActiveHolderDepartmentId",
                                            MIN(EA."IssueDate") as "ActiveHolderIssueDate",
                                            EA."ReturnDate" as "ActiveHolderReturnDate"

                                     FROM equipment."EquipmentAssigns" EA
                                              JOIN equipment."EquipmentUsers" EU ON EA."AssignedToUserId" = EU."UserId"

                                     WHERE  EA."IsDeleted" = FALSE
                                       AND (EA."ReturnDate" is null or EA."ReturnDate" > now())
                                       AND EA."IssueDate" <= now()

                                     GROUP BY EA."EquipmentId", EU."UserId", EU."FirstName", EU."LastName", EU."DepartmentId",EA."ReturnDate"

                                 ) AH ON E."Id" = AH."ActiveHolderEquipmentId"

                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    DROP VIEW equipment."EquipmentOdataView";
                                 """);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                schema: "equipment",
                table: "EquipmentUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                schema: "equipment",
                table: "EquipmentTypes",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "TypeId",
                schema: "equipment",
                table: "Equipments",
                type: "character varying(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Characteristics",
                schema: "equipment",
                table: "Equipments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.Sql("""
                                 CREATE OR REPLACE VIEW equipment."EquipmentOdataView" AS
                                 SELECT E."Id",
                                        "Name",
                                        "Location",
                                        "SerialNumber",
                                        "PurchasePrice",
                                        "PurchaseCurrency",
                                        "PurchasePriceUsd",
                                        "PurchaseDate",
                                        "PurchasePlace",
                                        "GuaranteeDate",
                                        "Characteristics",
                                        "Comment",
                                        "ApproverId",
                                        concat_ws(' ',EU."LastName", EU."FirstName") as "ApproverFullName",
                                        ET."Id"        as "EquipmentTypeId",
                                        ET."ShortName" as "EquipmentTypeShortName",
                                        ET."FullName"  as "EquipmentTypeFullName",
                                        AH."ActiveHolderId",
                                        concat_ws(' ',AH."ActiveHolderLastName",AH."ActiveHolderFirstName") as "ActiveHolderFullName",
                                        AH."ActiveHolderDepartmentId",
                                        AH."ActiveHolderIssueDate",
                                        AH."ActiveHolderReturnDate",
                                        "InvoiceNumber"

                                 FROM equipment."Equipments" E
                                          LEFT JOIN equipment."EquipmentUsers" EU ON E."ApproverId" = EU."UserId"
                                          JOIN equipment."EquipmentTypes" ET on ET."Id" = E."TypeId"
                                          LEFT JOIN (

                                     SELECT EA."EquipmentId" as "ActiveHolderEquipmentId",
                                            EU."UserId"           as "ActiveHolderId",
                                            EU."FirstName"    as "ActiveHolderFirstName",
                                            EU."LastName"     as "ActiveHolderLastName",
                                            EU."DepartmentId" as "ActiveHolderDepartmentId",
                                            MIN(EA."IssueDate") as "ActiveHolderIssueDate",
                                            EA."ReturnDate" as "ActiveHolderReturnDate"

                                     FROM equipment."EquipmentAssigns" EA
                                              JOIN equipment."EquipmentUsers" EU ON EA."AssignedToUserId" = EU."UserId"

                                     WHERE  EA."IsDeleted" = FALSE
                                       AND (EA."ReturnDate" is null or EA."ReturnDate" > now())
                                       AND EA."IssueDate" <= now()

                                     GROUP BY EA."EquipmentId", EU."UserId", EU."FirstName", EU."LastName", EU."DepartmentId",EA."ReturnDate"

                                 ) AH ON E."Id" = AH."ActiveHolderEquipmentId"

                                 """);
        }
    }
}

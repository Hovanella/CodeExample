using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Migrations
{
    /// <inheritdoc />
    public partial class EquipmentOdataView_AddFullNameToApproverAndActiveHolder_RemovedLastNameAndFullName : Migration
    {
                /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 DROP VIEW IF EXISTS equipment."EquipmentOdataView"
                                 """);

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
                                 DROP VIEW IF EXISTS equipment."EquipmentOdataView"
                                 """);

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
                                        EU."FirstName" as "ApproverFirstName",
                                        EU."LastName"  as "ApproverLastName",
                                        ET."Id"        as "EquipmentTypeId",
                                        ET."ShortName" as "EquipmentTypeShortName",
                                        ET."FullName"  as "EquipmentTypeFullName",
                                        AH."ActiveHolderId",
                                        AH."ActiveHolderFirstName",
                                        AH."ActiveHolderLastName",
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

using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Excel.Contract.Equipments;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Utils.Extensions;
using CustomerName.Portal.Identity.Contract.Contract;

namespace CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentExports;

internal interface IOdpEquipmentToEquipmentToExcelExportMapper
{
    List<EquipmentToExcelExport> Map(List<OdpEquipment> equipments, List<DepartmentDto> departments);
}

internal class OdpEquipmentToEquipmentToExcelExportMapper : IOdpEquipmentToEquipmentToExcelExportMapper
{
    public List<EquipmentToExcelExport> Map(List<OdpEquipment> equipments, List<DepartmentDto> departments)
    {
        return equipments.ConvertAll(x => new EquipmentToExcelExport
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.EquipmentTypeFullName,
            SerialNumber = x.SerialNumber,
            PurchasePriceUsd = x.PurchasePriceUsd,
            PurchaseDate = x.PurchaseDate,
            PurchasePlace = x.PurchasePlace,
            GuaranteeDate = x.GuaranteeDate,
            Characteristics = x.Characteristics,
            Availability = x.ActiveHolderId is null
                ? EquipmentAvailability.Available
                : EquipmentAvailability.Issued,
            Location = Enum.Parse<EquipmentLocationType>(x.Location).GetDescription(),
            Comment = x.Comment,
            InvoiceNumber = x.InvoiceNumber,
            Approver = new ApproverToExcelExport
            {
                ApproverId = x.Id, ApproverFullName = x.ApproverFullName
            },
            ActiveHolder = x is { ActiveHolderIssueDate: not null, ActiveHolderId: not null }
                ? new EquipmentHolderToExcelExport
                {
                    Id = x.Id,
                    FullName = x.ActiveHolderFullName!,
                    DepartmentFullName = departments.FirstOrDefault(d => d.Id == x.ActiveHolderDepartmentId)?.FullName,
                }
                : null
        });
    }
}

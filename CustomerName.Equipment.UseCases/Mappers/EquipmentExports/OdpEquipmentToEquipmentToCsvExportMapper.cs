using CustomerName.Portal.Csv.Contract.Equipments;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Utils.Extensions;
using CustomerName.Portal.Identity.Contract.Contract;

namespace CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentExports;

internal interface IOdpEquipmentToEquipmentToCsvExportMapper
{
    List<EquipmentToCsvExport> Map(List<OdpEquipment> equipments, List<DepartmentDto> departments);
}

internal class OdpEquipmentToEquipmentToCsvExportMapper : IOdpEquipmentToEquipmentToCsvExportMapper
{
    public List<EquipmentToCsvExport> Map(List<OdpEquipment> equipments, List<DepartmentDto> departments)
    {
        return equipments.ConvertAll(x => new EquipmentToCsvExport
        {
            Id = x.Id,
            Name = x.Name,
            TypeFullName = x.EquipmentTypeFullName,
            SerialNumber = x.SerialNumber,
            PurchasePriceUsd = x.PurchasePriceUsd,
            PurchaseDate = x.PurchaseDate,
            PurchasePlace = x.PurchasePlace,
            GuaranteeDate = x.GuaranteeDate,
            Characteristics = x.Characteristics,
            Availability = x.ActiveHolderId is null
                ? EquipmentAvailability.Available
                : EquipmentAvailability.Issued,
            InvoiceNumber = x.InvoiceNumber,
            Location = Enum.Parse<EquipmentLocationType>(x.Location).GetDescription(),
            Comment = x.Comment,
            Approver = new ApproverToCsvExport { Id = x.Id, FullName = x.ApproverFullName! },
            ActiveHolder = x is { ActiveHolderIssueDate: not null, ActiveHolderId: not null }
                ? new ActiveHolderToCsvExport
                {
                    Id = x.Id,
                    FullName = x.ActiveHolderFullName!,
                    DepartmentFullName = departments.FirstOrDefault(d => d.Id == x.ActiveHolderDepartmentId)?.FullName,
                }
                : null
        });
    }
}

using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Utils.Extensions;

namespace CustomerName.Portal.Equipment.Controllers.Mappers;

internal interface IEquipmentDtoToEquipmentOutputMapper
{
    EquipmentOutput Map(EquipmentDto dto);
}

internal class EquipmentDtoToEquipmentOutputMapper : IEquipmentDtoToEquipmentOutputMapper
{
    public EquipmentOutput Map(EquipmentDto dto)
    {
        return new EquipmentOutput
        {
            Id = dto.Id,
            Name = dto.Name,
            Location = dto.Location.GetDescription(),
            SerialNumber = dto.SerialNumber,
            PurchasePrice = dto.PurchasePrice,
            PurchaseCurrency = dto.PurchaseCurrency.GetDescription(),
            PurchasePriceAndCurrency = $"{dto.PurchasePrice} {dto.PurchaseCurrency.GetDescription()}",
            PurchasePriceUsd = dto.PurchasePriceUsd,
            PurchaseDate = dto.PurchaseDate,
            PurchasePlace = dto.PurchasePlace,
            GuaranteeDate = dto.GuaranteeDate,
            Characteristics = dto.Characteristics,
            Comment = dto.Comment,
            InvoiceNumber = dto.InvoiceNumber,
            EquipmentTypeId = dto.TypeId,
            Approver = new ApproverOutput
            {
                Id = dto.Approver!.Id, FullName = $"{dto.Approver.LastName} {dto.Approver.FirstName}",
            },
            ActiveHolder = dto.ActiveHolder != null
                ? new EquipmentHolderOutput
                {
                    Id = dto.ActiveHolder.Id,
                    FullName = $"{dto.ActiveHolder.LastName} {dto.ActiveHolder.FirstName}",
                    DepartmentId = dto.ActiveHolder.DepartmentId,
                    IssueDate = dto.ActiveHolder.IssueDate,
                    ReturnDate = dto.ActiveHolder.ReturnDate
                }
                : null,
            Type = new EquipmentTypeOutput
            (
                dto.Type!.Id,
                dto.Type.ShortName,
                dto.Type.FullName
            )
        };
    }
}

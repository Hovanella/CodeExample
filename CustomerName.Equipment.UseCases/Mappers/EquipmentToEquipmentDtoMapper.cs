using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappers;

internal interface IEquipmentToEquipmentDtoMapper
{
    EquipmentDto Map(Domain.Equipment equipment);
}

internal class EquipmentToEquipmentDtoMapper : IEquipmentToEquipmentDtoMapper
{
    public EquipmentDto Map(Domain.Equipment equipment)
    {
        return new EquipmentDto()
        {
            Id = equipment.Id,
            Name = equipment.Name,
            SerialNumber = equipment.SerialNumber,
            TypeId = equipment.TypeId,
            PurchasePrice = equipment.PurchasePrice,
            PurchaseCurrency = equipment.PurchaseCurrency,
            PurchasePriceUsd = equipment.PurchasePriceUsd,
            Location = equipment.Location,
            PurchaseDate = equipment.PurchaseDate,
            PurchasePlace = equipment.PurchasePlace,
            GuaranteeDate = equipment.GuaranteeDate,
            Characteristics = equipment.Characteristics,
            Comment = equipment.Comment,
            InvoiceNumber = equipment.InvoiceNumber,
            Approver = new ApproverDto
            {
                Id = equipment.ApproverId,
                FirstName = equipment.Approver.FirstName,
                LastName = equipment.Approver.LastName
            },
            Type = new EquipmentTypeDto(equipment.Type.Id,
                                        equipment.Type.ShortName,
                                        equipment.Type.FullName)
        };
    }
}

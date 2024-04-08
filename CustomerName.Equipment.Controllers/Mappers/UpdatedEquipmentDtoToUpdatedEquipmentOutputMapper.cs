using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Utils.Extensions;

namespace CustomerName.Portal.Equipment.Controllers.Mappers;

internal interface IUpdatedEquipmentDtoToUpdatedEquipmentOutputMapper
{
    UpdatedEquipmentOutput Map(UpdatedEquipmentDto equipmentDto);
}

internal class UpdatedEquipmentDtoToUpdatedEquipmentOutputMapper : IUpdatedEquipmentDtoToUpdatedEquipmentOutputMapper
{
    public UpdatedEquipmentOutput Map(UpdatedEquipmentDto equipmentDto)
    {
        return new UpdatedEquipmentOutput(
            equipmentDto.Id,
            equipmentDto.Name,
            equipmentDto.TypeId,
            equipmentDto.Location.ToString(),
            equipmentDto.SerialNumber,
            equipmentDto.PurchasePrice,
            equipmentDto.PurchaseCurrency.ToString(),
            $"{equipmentDto.PurchasePrice} {equipmentDto.PurchaseCurrency.GetDescription()}",
            equipmentDto.PurchasePriceUsd,
            equipmentDto.PurchaseDate,
            equipmentDto.PurchasePlace,
            equipmentDto.GuaranteeDate,
            equipmentDto.Characteristics,
            equipmentDto.Comment,
            equipmentDto.InvoiceNumber,
            equipmentDto.ApproverId);
    }
}

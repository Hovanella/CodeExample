using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.UpdateEquipmentDbQuery;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappers;

internal interface IUpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper
{
    UpdatedEquipmentDto Map(UpdateEquipmentDbQueryRequest dbQueryRequest);
}

internal class UpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper : IUpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper
{
    public UpdatedEquipmentDto Map(UpdateEquipmentDbQueryRequest dbQueryRequest)
    {
        return new UpdatedEquipmentDto(
            dbQueryRequest.Id,
            dbQueryRequest.Name,
            dbQueryRequest.TypeId,
            dbQueryRequest.Location,
            dbQueryRequest.SerialNumber,
            dbQueryRequest.PurchasePrice,
            dbQueryRequest.PurchaseCurrency,
            dbQueryRequest.PurchasePriceUsd,
            dbQueryRequest.PurchaseDate,
            dbQueryRequest.PurchasePlace,
            dbQueryRequest.GuaranteeDate,
            dbQueryRequest.Characteristics,
            dbQueryRequest.Comment,
            dbQueryRequest.InvoiceNumber,
            dbQueryRequest.ApproverId
        );
    }
}

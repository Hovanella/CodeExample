using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.UpdateEquipmentDbQuery;
using CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;

namespace CustomerName.Portal.Equipment.UseCases.Mappers;

internal interface IUpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper
{
    UpdateEquipmentDbQueryRequest Map(UpdateEquipmentCommand request, string typeId, int approverId);
}

internal class UpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper : IUpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper
{
    public UpdateEquipmentDbQueryRequest Map(UpdateEquipmentCommand request, string typeId, int approverId ) {
        return new UpdateEquipmentDbQueryRequest(
            request.Id,
            request.Name,
            typeId,
            request.Location,
            request.SerialNumber,
            request.PurchasePrice,
            request.PurchaseCurrency,
            request.PurchasePriceUsd,
            request.PurchaseDate,
            request.PurchasePlace,
            request.GuaranteeDate,
            request.Characteristics,
            request.Comment,
            request.InvoiceNumber,
            approverId);
    }
}

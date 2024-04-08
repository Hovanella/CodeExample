using CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;

namespace CustomerName.Portal.Equipment.UseCases.Mappers;

internal interface IUpdateEquipmentCommandToEquipmentMapper
{
    void Map(UpdateEquipmentCommand command, Domain.Equipment equipment);
}

internal class UpdateEquipmentCommandToEquipmentMapper : IUpdateEquipmentCommandToEquipmentMapper
{
    public void Map(UpdateEquipmentCommand command, Domain.Equipment equipment)
    {
        equipment.Name = command.Name;
        equipment.TypeId = command.TypeId;
        equipment.Location = command.Location;
        equipment.SerialNumber = command.SerialNumber;
        equipment.PurchasePrice = command.PurchasePrice;
        equipment.PurchaseCurrency = command.PurchaseCurrency;
        equipment.PurchasePriceUsd = command.PurchasePriceUsd;
        equipment.PurchaseDate = command.PurchaseDate;
        equipment.PurchasePlace = command.PurchasePlace;
        equipment.GuaranteeDate = command.GuaranteeDate;
        equipment.Characteristics = command.Characteristics;
        equipment.Comment = command.Comment;
        equipment.InvoiceNumber = command.InvoiceNumber;
    }
}

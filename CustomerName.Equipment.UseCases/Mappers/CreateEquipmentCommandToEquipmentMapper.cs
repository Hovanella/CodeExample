using CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipment;

namespace CustomerName.Portal.Equipment.UseCases.Mappers;

internal interface ICreateEquipmentCommandToEquipmentMapper
{
    Domain.Equipment Map(CreateEquipmentCommand command);
}

internal class CreateEquipmentCommandToEquipmentMapper : ICreateEquipmentCommandToEquipmentMapper
{
    public Domain.Equipment Map(CreateEquipmentCommand command)
    {
        return new Domain.Equipment
        {
            SerialNumber = command.SerialNumber,
            Name = command.Name,
            Location = command.Location,
            PurchasePrice = command.PurchasePrice,
            PurchaseCurrency = command.PurchaseCurrency,
            PurchasePriceUsd = command.PurchasePriceUsd,
            PurchaseDate = command.PurchaseDate,
            PurchasePlace = command.PurchasePlace,
            GuaranteeDate = command.GuaranteeDate,
            Characteristics = command.Characteristics,
            Comment = command.Comment,
            InvoiceNumber = command.InvoiceNumber,
            ApproverId = command.ApproverId,
            TypeId = command.TypeId,
        };
    }
}

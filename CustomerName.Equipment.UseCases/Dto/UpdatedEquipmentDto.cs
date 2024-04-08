using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.UseCases.Dto;

internal record UpdatedEquipmentDto(
    int Id,
    string Name,
    string TypeId,
    EquipmentLocationType Location,
    string SerialNumber,
    decimal PurchasePrice,
    MoneyCurrencyType PurchaseCurrency,
    decimal PurchasePriceUsd,
    DateTime PurchaseDate,
    string PurchasePlace,
    DateTime GuaranteeDate,
    string Characteristics,
    string? Comment,
    string? InvoiceNumber,
    int ApproverId);

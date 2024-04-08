using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.UpdateEquipmentDbQuery;

public record UpdateEquipmentDbQueryRequest(
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

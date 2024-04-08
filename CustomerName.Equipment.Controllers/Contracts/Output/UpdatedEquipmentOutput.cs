namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

internal record UpdatedEquipmentOutput(
    int Id,
    string Name,
    string EquipmentTypeId,
    string Location,
    string SerialNumber,
    decimal PurchasePrice,
    string PurchaseCurrency,
    string PurchasePriceAndCurrency,
    decimal PurchasePriceUsd,
    DateTime PurchaseDate,
    string PurchasePlace,
    DateTime GuaranteeDate,
    string Characteristics,
    string? Comment,
    string? InvoiceNumber,
    int ApproverId);

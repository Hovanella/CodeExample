using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Framework.UseCases.Abstractions.Transactions;

namespace CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;

internal record UpdateEquipmentCommand(
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
    string InvoiceNumber,
    int ApproverId) : ICommand<UpdatedEquipmentDto>, IEquipmentRequest, ITransactionalRequest;

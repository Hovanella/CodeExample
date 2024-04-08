using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Framework.UseCases.Abstractions.Transactions;

namespace CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipment;

internal record CreateEquipmentCommand(
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
    int ApproverId) : ICommand<EquipmentDto>, IEquipmentRequest, ITransactionalRequest;

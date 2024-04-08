using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Framework.UseCases.Abstractions.Transactions;

namespace CustomerName.Portal.Equipment.UseCases.Commands.DeleteEquipmentHistoryRecord;

internal record DeleteEquipmentHistoryCommand(int EquipmentId)
    : ICommand<EquipmentHolderDto>, IEquipmentRequest, ITransactionalRequest;

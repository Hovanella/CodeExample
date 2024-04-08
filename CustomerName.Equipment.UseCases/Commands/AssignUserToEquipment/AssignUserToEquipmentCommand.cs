using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Framework.UseCases.Abstractions.Transactions;

namespace CustomerName.Portal.Equipment.UseCases.Commands.AssignUserToEquipment;

internal record AssignUserToEquipmentCommand(
    int EquipmentId,
    int UserId,
    DateTime IssueDate) : ICommand<ActiveHolderDto>, IEquipmentRequest, ITransactionalRequest;

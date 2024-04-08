using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Framework.UseCases.Abstractions.Transactions;

namespace CustomerName.Portal.Equipment.UseCases.Commands.PopulateReturnDate;

internal record SetReturnDateToAssignCommand(
    int EquipmentId,
    DateTime ReturnDate) : ICommand<ActiveHolderDto>, IEquipmentRequest, ITransactionalRequest;

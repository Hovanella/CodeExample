using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetAssignsByEquipmentId;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.SetReturnDateToAssign;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;

namespace CustomerName.Portal.Equipment.UseCases.Commands.PopulateReturnDate;

internal class SetReturnDateToAssignCommandHandler : ICommandHandler<SetReturnDateToAssignCommand, ActiveHolderDto>
{
    private readonly IIsEquipmentExistByIdDbQuery _isEquipmentExistByIdDbQuery;
    private readonly IGetAssignHolderByAssignIdDbQuery _getAssignHolderByAssignIdDbQuery;
    private readonly ISetReturnDateToAssignDbQuery _setReturnDateToAssignDbQuery;
    private readonly IGetAssignsByEquipmentIdDbQuery _getAssignsByEquipmentIdDbQuery;

    public SetReturnDateToAssignCommandHandler(
        IIsEquipmentExistByIdDbQuery isEquipmentExistByIdDbQuery,
        IGetAssignsByEquipmentIdDbQuery getAssignsByEquipmentIdDbQuery,
        ISetReturnDateToAssignDbQuery setReturnDateToAssignAndReturnActiveHolderDbQuery,
        IGetAssignHolderByAssignIdDbQuery getAssignHolderByAssignIdDbQuery)
    {
        _isEquipmentExistByIdDbQuery = isEquipmentExistByIdDbQuery;
        _getAssignsByEquipmentIdDbQuery = getAssignsByEquipmentIdDbQuery;
        _setReturnDateToAssignDbQuery = setReturnDateToAssignAndReturnActiveHolderDbQuery;
        _getAssignHolderByAssignIdDbQuery = getAssignHolderByAssignIdDbQuery;
    }

    public async Task<ActiveHolderDto> Handle(
        SetReturnDateToAssignCommand request,
        CancellationToken cancellationToken)
    {
        var equipmentEntity = await _isEquipmentExistByIdDbQuery.ExecuteAsync(
            request.EquipmentId,
            cancellationToken);

        if (!equipmentEntity)
        {
            throw new EntityNotFoundException(ExceptionConstants.EquipmentNotFound);
        }

        var lastAssign = await ValidateAssignDatesAndReturnLastAssign(request,cancellationToken);

        await _setReturnDateToAssignDbQuery.ExecuteAsync(
            new SetReturnDateToAssignRequest(lastAssign.Id, request.ReturnDate),
            cancellationToken);

        return await _getAssignHolderByAssignIdDbQuery.ExecuteAsync(lastAssign.Id, cancellationToken);
    }

    private async Task<EquipmentAssign> ValidateAssignDatesAndReturnLastAssign(
        SetReturnDateToAssignCommand request,
        CancellationToken cancellationToken)
    {
        var assigns = await _getAssignsByEquipmentIdDbQuery.ExecuteAsync(
            request.EquipmentId,
            cancellationToken);

        if (assigns.Count == 0)
        {
            throw new InvalidDataAppException(ExceptionConstants.EquipmentWasNeverAssigned);
        }

        if (assigns.Count(x => x.ReturnDate == null) > 1)
        {
            throw new InvalidDataAppException(ExceptionConstants.SeveralNullDatesForEquipment);
        }

        var lastAssign = assigns.OrderByDescending(assign => assign.IssueDate)
                                .ThenByDescending(assign => assign.Id)
                                .First();

        if (lastAssign.ReturnDate != null)
        {
            throw new EntityConflictException(ExceptionConstants.ExistingReturnDateForLastAssignment);
        }

        if (lastAssign.IssueDate > request.ReturnDate)
        {
            throw new InvalidDataAppException(ExceptionConstants.ReturnDateLowerThanIssueDate);
        }

        return lastAssign;
    }
}



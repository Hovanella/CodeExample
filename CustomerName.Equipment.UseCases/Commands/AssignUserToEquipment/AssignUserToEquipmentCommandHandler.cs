using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentAssign;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;

namespace CustomerName.Portal.Equipment.UseCases.Commands.AssignUserToEquipment;

internal class AssignUserToEquipmentCommandHandler : ICommandHandler<AssignUserToEquipmentCommand, ActiveHolderDto>
{
    private readonly IIsEquipmentExistByIdDbQuery _isEquipmentExistByIdDbQuery;
    private readonly IGetEquipmentUserByIdDbQuery _getEquipmentUserByIdDbQuery;
    private readonly ICreateEquipmentAssignDbQuery _createEquipmentAssignDbQuery;
    private readonly IGetAssignsReturnDatesByEquipmentIdDbQuery _getAssignsReturnDatesByEquipmentIdDbQuery;

    public AssignUserToEquipmentCommandHandler(
        IIsEquipmentExistByIdDbQuery isEquipmentExistByIdDbQuery,
        IGetEquipmentUserByIdDbQuery getEquipmentUserByIdDbQuery,
        ICreateEquipmentAssignDbQuery createEquipmentAssignDbQuery,
        IGetAssignsReturnDatesByEquipmentIdDbQuery getAssignsReturnDatesByEquipmentIdDbQuery)
    {
        _isEquipmentExistByIdDbQuery = isEquipmentExistByIdDbQuery;
        _getEquipmentUserByIdDbQuery = getEquipmentUserByIdDbQuery;
        _createEquipmentAssignDbQuery = createEquipmentAssignDbQuery;
        _getAssignsReturnDatesByEquipmentIdDbQuery = getAssignsReturnDatesByEquipmentIdDbQuery;
    }

    public async Task<ActiveHolderDto> Handle(
        AssignUserToEquipmentCommand request,
        CancellationToken cancellationToken)
    {
       var isEquipmentExist = await _isEquipmentExistByIdDbQuery.ExecuteAsync(
           request.EquipmentId,
           cancellationToken);
       if (!isEquipmentExist)
       {
           throw new EntityNotFoundException(ExceptionConstants.EquipmentNotFound);
       }

       var user = await _getEquipmentUserByIdDbQuery.ExecuteAsync(request.UserId, cancellationToken)
           ?? throw new EntityNotFoundException(ExceptionConstants.UserNotFound);

        var equipmentAssignsReturnDates = await _getAssignsReturnDatesByEquipmentIdDbQuery
            .ExecuteAsync(request.EquipmentId, cancellationToken);

        if (equipmentAssignsReturnDates.Length != 0)
        {
            ValidateAssignDates(request, equipmentAssignsReturnDates);
        }

        var createdEquipmentAssignment = await _createEquipmentAssignDbQuery.ExecuteAsync(
            new EquipmentAssign
            {
                AssignedToUserId = user.UserId,
                EquipmentId = request.EquipmentId,
                IssueDate = request.IssueDate,
            },
            cancellationToken);

        return new ActiveHolderDto
        {
            Id = createdEquipmentAssignment.UserId,
            FirstName = user!.FirstName,
            LastName = user.LastName,
            DepartmentId = user.DepartmentId,
            IssueDate = createdEquipmentAssignment.IssueDate,
            ReturnDate = null
        };
    }

    private static void ValidateAssignDates(AssignUserToEquipmentCommand request, DateTime?[] equipmentAssignsReturnDates)
    {
        if (equipmentAssignsReturnDates.Any(x=>x is null))
        {
            throw new EntityConflictException(ExceptionConstants.EquipmentAlreadyAssigned);
        }

        var maxReturnDate = equipmentAssignsReturnDates.Max()!.Value;
        if (maxReturnDate.Date > request.IssueDate.Date)
        {
            throw new EntityConflictException(ExceptionConstants.EquipmentAlreadyAssigned);
        }
    }
}

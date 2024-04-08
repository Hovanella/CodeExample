using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipment;

internal class CreateEquipmentCommandHandler : ICommandHandler<CreateEquipmentCommand, EquipmentDto>
{
    private readonly IIdentityContract _identityContract;
    private readonly IIsEquipmentTypeExistDbQuery _isEquipmentTypeExistDbQuery;
    private readonly IIsEquipmentUserExistDbQuery _isEquipmentUserExistDbQuery;
    private readonly IIsSerialNumberExistingDbQuery _isSerialNumberExistingDbQuery;
    private readonly IGetEquipmentApproverAndTypeByIdDbQuery _getEquipmentApproverAndTypeByIdDbQuery;
    private readonly ICreateEquipmentDbQuery _createEquipmentDbQuery;
    private readonly IEquipmentToEquipmentDtoMapper _equipmentToEquipmentDtoMapper;
    private readonly ICreateEquipmentCommandToEquipmentMapper _createEquipmentCommandToEquipmentMapper;

    public CreateEquipmentCommandHandler(
        IIdentityContract identityContract,
        IIsEquipmentTypeExistDbQuery isEquipmentTypeExistDbQuery,
        IIsEquipmentUserExistDbQuery isEquipmentUserExistDbQuery,
        IGetEquipmentApproverAndTypeByIdDbQuery getEquipmentApproverAndTypeByIdDbQuery,
        ICreateEquipmentDbQuery createEquipmentDbQuery,
        IEquipmentToEquipmentDtoMapper equipmentToEquipmentDtoMapper,
        ICreateEquipmentCommandToEquipmentMapper createEquipmentCommandToEquipmentMapper,
        IIsSerialNumberExistingDbQuery isSerialNumberExistingDbQuery)
    {
        _identityContract = identityContract;
        _isEquipmentTypeExistDbQuery = isEquipmentTypeExistDbQuery;
        _isEquipmentUserExistDbQuery = isEquipmentUserExistDbQuery;
        _getEquipmentApproverAndTypeByIdDbQuery = getEquipmentApproverAndTypeByIdDbQuery;
        _createEquipmentDbQuery = createEquipmentDbQuery;
        _equipmentToEquipmentDtoMapper = equipmentToEquipmentDtoMapper;
        _createEquipmentCommandToEquipmentMapper = createEquipmentCommandToEquipmentMapper;
        _isSerialNumberExistingDbQuery = isSerialNumberExistingDbQuery;
    }

    public async Task<EquipmentDto> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        await ValidateRequest(request, cancellationToken);

        var equipmentToAdd = _createEquipmentCommandToEquipmentMapper.Map(request);

        var createdEquipment = await _createEquipmentDbQuery.CreateEquipmentAsync(equipmentToAdd, cancellationToken);

        var equipment = await _getEquipmentApproverAndTypeByIdDbQuery.ExecuteAsync(createdEquipment.Id, cancellationToken);

        createdEquipment.Approver = equipment.Approver;
        createdEquipment.Type = equipment.Type;

        return _equipmentToEquipmentDtoMapper.Map(createdEquipment);
    }

    private async Task ValidateRequest(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var isUserExisting = await _isEquipmentUserExistDbQuery
                                   .ExecuteAsync(request.ApproverId, cancellationToken);
        if (!isUserExisting)
        {
            throw new EntityNotFoundException(ExceptionConstants.ApproverDoesNotExist);
        }

        var managerRoles = new List<RoleType> { RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment };

        var isApproverManager = await _identityContract
                                      .HasUserAnyRoleAsync(request.ApproverId, managerRoles, cancellationToken);

        if (!isApproverManager)
        {
            throw new ActionNotAllowedException(ExceptionConstants.ApproverDoesNotHavePermission);
        }

        if (!await _isEquipmentTypeExistDbQuery.IsEquipmentTypeExist(request.TypeId, cancellationToken))
        {
            throw new EntityNotFoundException(ExceptionConstants.EquipmentTypeNotFound);
        }

        var isSerialNumberExistingRequest = new IsSerialNumberExistingDbRequest(request.SerialNumber, null);

        if (await _isSerialNumberExistingDbQuery.ExecuteAsync(isSerialNumberExistingRequest, cancellationToken))
        {
            throw new InvalidDataAppException(ExceptionConstants.ExistingSerialNumber);
        }
    }
}

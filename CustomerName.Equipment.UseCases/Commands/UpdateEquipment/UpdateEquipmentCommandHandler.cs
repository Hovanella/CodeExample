using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.UpdateEquipmentDbQuery;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;

internal class UpdateEquipmentCommandHandler : ICommandHandler<UpdateEquipmentCommand, UpdatedEquipmentDto>
{
    private readonly IUpdateEquipmentDbQuery _updateEquipmentDbQuery;
    private readonly IIsEquipmentExistByIdDbQuery _isEquipmentExistByIdDbQuery;
    private readonly IIsSerialNumberExistingDbQuery _isSerialNumberExistingDbQuery;
    private readonly IGetEquipmentApproverDbQuery _getEquipmentApproverDbQuery;
    private readonly IIsEquipmentTypeExistDbQuery _isEquipmentTypeExistDbQuery;
    private readonly IGetEquipmentTypeByIdDbQuery _getEquipmentTypeByIdDbQuery;
    private readonly IUpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper _updateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper;
    private readonly IUpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper _updateEquipmentDbQueryToUpdatedEquipmentDtoMapper;

    private readonly IIdentityContract _identityContract;

    public UpdateEquipmentCommandHandler(
        IIdentityContract identityContract,
        IIsEquipmentTypeExistDbQuery isEquipmentTypeExistDbQuery,
        IGetEquipmentTypeByIdDbQuery getEquipmentTypeByIdDbQuery,
        IUpdateEquipmentDbQuery updateEquipmentDbQuery,
        IUpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper updateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper,
        IGetEquipmentApproverDbQuery getEquipmentApproverDbQuery,
        IIsEquipmentExistByIdDbQuery isEquipmentExistByIdDbQuery,
        IUpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper updateEquipmentDbQueryToUpdatedEquipmentDtoMapper,
        IIsSerialNumberExistingDbQuery isSerialNumberExistingDbQuery)
    {
        _identityContract = identityContract;
        _isEquipmentTypeExistDbQuery = isEquipmentTypeExistDbQuery;
        _getEquipmentTypeByIdDbQuery = getEquipmentTypeByIdDbQuery;
        _updateEquipmentDbQuery = updateEquipmentDbQuery;
        _updateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper = updateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper;
        _getEquipmentApproverDbQuery = getEquipmentApproverDbQuery;
        _isEquipmentExistByIdDbQuery = isEquipmentExistByIdDbQuery;
        _updateEquipmentDbQueryToUpdatedEquipmentDtoMapper = updateEquipmentDbQueryToUpdatedEquipmentDtoMapper;
        _isSerialNumberExistingDbQuery = isSerialNumberExistingDbQuery;
    }

    public async Task<UpdatedEquipmentDto> Handle(
        UpdateEquipmentCommand request,
        CancellationToken cancellationToken)
    {
        var equipmentApprover = await _getEquipmentApproverDbQuery.ExecuteAsync(
            request.ApproverId,
            cancellationToken) ?? throw new EntityNotFoundException(ExceptionConstants.ApproverDoesNotExist);

        await CheckIfApproverHasPermissionsAsync(
            equipmentApprover.UserId,
            cancellationToken);

        await ValidateRequest(request, cancellationToken);

        var dbQueryRequest = await CreateDbQueryRequest(request,equipmentApprover, cancellationToken);

        await _updateEquipmentDbQuery.ExecuteAsync(dbQueryRequest, cancellationToken);

        return _updateEquipmentDbQueryToUpdatedEquipmentDtoMapper.Map(dbQueryRequest);
    }

    private async Task<UpdateEquipmentDbQueryRequest> CreateDbQueryRequest(
        UpdateEquipmentCommand request,
        EquipmentUser equipmentApprover,
        CancellationToken cancellationToken)
    {
        var equipmentType= await _getEquipmentTypeByIdDbQuery.GetEquipmentTypeByIdAsync(
            request.TypeId,
            cancellationToken);

        var updateEquipmentDbQueryRequest = _updateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper
            .Map(request,equipmentType.Id,equipmentApprover.UserId);

        return updateEquipmentDbQueryRequest;
    }

    private async Task ValidateRequest(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        if (!await _isEquipmentTypeExistDbQuery.IsEquipmentTypeExist(request.TypeId, cancellationToken))
        {
            throw new EntityNotFoundException(ExceptionConstants.EquipmentTypeNotFound);
        }

        if (!await _isEquipmentExistByIdDbQuery.ExecuteAsync(request.Id, cancellationToken))
        {
            throw new EntityNotFoundException(ExceptionConstants.EquipmentNotFound);
        }

        var isSerialNumberExistingRequest = new IsSerialNumberExistingDbRequest(request.SerialNumber, request.Id);

        if (await _isSerialNumberExistingDbQuery.ExecuteAsync(isSerialNumberExistingRequest, cancellationToken))
        {
            throw new InvalidDataAppException(ExceptionConstants.ExistingSerialNumber);
        }
    }

    private async Task CheckIfApproverHasPermissionsAsync(
        int approverId,
        CancellationToken cancellationToken)
    {
        var managerRoles = new List<RoleType> { RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment };

        var isApproverManager = await _identityContract.HasUserAnyRoleAsync(
            approverId,
            managerRoles,
            cancellationToken);

        if (!isApproverManager)
        {
            throw new ActionNotAllowedException(ExceptionConstants.ApproverDoesNotHavePermission);
        }
    }
}


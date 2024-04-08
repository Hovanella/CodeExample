using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentById;

internal class GetEquipmentByIdQueryHandler : IQueryHandler<GetEquipmentByIdQuery, EquipmentDto>
{
    private readonly IEquipmentProvider _equipmentProvider;
    private readonly IMapper _mapper;
    private readonly IClockService _clockService;
    private readonly IAuthenticatedUserContext _authenticatedUserContext;

    public GetEquipmentByIdQueryHandler(
        IEquipmentProvider equipmentProvider,
        IMapper mapper,
        IClockService clockService,
        IAuthenticatedUserContext authenticatedUserContext)
    {
        _equipmentProvider = equipmentProvider;
        _mapper = mapper;
        _clockService = clockService;
        _authenticatedUserContext = authenticatedUserContext;
    }

    public async Task<EquipmentDto> Handle(
        GetEquipmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var equipment = await _equipmentProvider.GetEquipmentByIdAsync(
            request.Id,
            cancellationToken)
                ?? throw new EntityNotFoundException(ExceptionConstants.EquipmentNotFound);

        var activeHolder = equipment.GetActiveAssign(_clockService.UtcNow);

        if (_authenticatedUserContext.Role == RoleType.HeadOfDepartment &&
            (activeHolder is null ||
             activeHolder.User!.DepartmentId is null ||
             activeHolder.User.DepartmentId != _authenticatedUserContext.DepartmentId))
        {
            throw new ActionNotAllowedException(ExceptionConstants.HeadCannotSeeOtherDepartmentEquipment);
        }

        var dto = _mapper.Map<EquipmentDto>(equipment);

        if (_authenticatedUserContext.Role is RoleType.HeadOfDepartment or RoleType.Employee)
        {
            dto.PurchasePlace = null;
            dto.PurchasePriceUsd = 0;
            dto.PurchasePrice = 0;
        }

        if (activeHolder is not null)
        {
            dto.ActiveHolder = _mapper.Map<EquipmentHolderDto>(activeHolder);
        }

        return dto;
    }
}

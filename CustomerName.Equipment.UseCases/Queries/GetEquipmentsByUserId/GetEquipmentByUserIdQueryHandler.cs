using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByUserId;

internal class GetEquipmentByUserIdQueryHandler : IQueryHandler<GetEquipmentByUserIdQuery, List<UserEquipmentDto>>
{
    private readonly IEquipmentAssignProvider _assignEquipmentProvider;
    private readonly IMapper _mapper;
    private readonly IEquipmentUserProvider _equipmentUserProvider;
    private readonly IIsAllowedToGetUserEquipmentBusinessRule _isAllowedToGetUserEquipmentBusinessRule;

    public GetEquipmentByUserIdQueryHandler(
        IEquipmentAssignProvider assignEquipmentProvider,
        IMapper mapper,
        IEquipmentUserProvider equipmentUserProvider,
        IIsAllowedToGetUserEquipmentBusinessRule isAllowedToGetUserEquipmentBusinessRule)
    {
        _assignEquipmentProvider = assignEquipmentProvider;
        _mapper = mapper;
        _equipmentUserProvider = equipmentUserProvider;
        _isAllowedToGetUserEquipmentBusinessRule = isAllowedToGetUserEquipmentBusinessRule;
    }

    public async Task<List<UserEquipmentDto>> Handle(
        GetEquipmentByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _equipmentUserProvider.GetUserByIdAsync(
            request.UserId,
            cancellationToken)
                ?? throw new EntityNotFoundException(ExceptionConstants.UserNotFound);

        if (!await _isAllowedToGetUserEquipmentBusinessRule.IsCorrespondsToBusiness(request.UserId, cancellationToken))
        {
            throw new ActionNotAllowedException(ExceptionConstants.YouDoNotHavePermission);
        }

        var equipments = await _assignEquipmentProvider.GetEquipmentsByUserIdAsync(
            user.UserId,
            cancellationToken);

        return _mapper.Map<List<UserEquipmentDto>>(equipments);
    }
}

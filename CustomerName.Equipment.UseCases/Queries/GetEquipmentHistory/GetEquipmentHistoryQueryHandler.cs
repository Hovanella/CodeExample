using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentHistory;

internal class GetEquipmentHistoryQueryHandler : IQueryHandler<GetEquipmentHistoryQuery, List<EquipmentHolderDto>>
{
    private readonly IEquipmentAssignProvider _equipmentAssignProvider;
    private readonly IMapper _mapper;

    public GetEquipmentHistoryQueryHandler(
        IEquipmentAssignProvider equipmentAssignProvider,
        IMapper mapper)
    {
        _equipmentAssignProvider = equipmentAssignProvider;
        _mapper = mapper;
    }

    public async Task<List<EquipmentHolderDto>> Handle(
        GetEquipmentHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var equipmentAssigns = await _equipmentAssignProvider.GetByEquipmentIdAsync(
            request.EquipmentId,
            cancellationToken);

        return _mapper.Map<List<EquipmentHolderDto>>(equipmentAssigns);
    }
}

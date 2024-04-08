using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Builders;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Providers;

internal class EquipmentProvider : IEquipmentProvider
{
    private readonly IMapper _mapper;
    private readonly IEquipmentQueryBuilder _equipmentQueryBuilder;

    public EquipmentProvider(
        IMapper mapper,
        IEquipmentQueryBuilder equipmentQueryBuilder)
    {
        _mapper = mapper;
        _equipmentQueryBuilder = equipmentQueryBuilder;
    }

    public async Task<Domain.Equipment?> GetEquipmentByIdAsync(
        int equipmentId,
        CancellationToken cancellationToken)
    {
        var equipment = await _equipmentQueryBuilder.Build()
            .FirstOrDefaultAsync(equipment => equipment.Id == equipmentId, cancellationToken);

        return _mapper.Map<Domain.Equipment?>(equipment);
    }
}

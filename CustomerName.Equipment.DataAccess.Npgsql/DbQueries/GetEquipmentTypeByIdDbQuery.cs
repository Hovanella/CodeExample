using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetEquipmentTypeByIdDbQuery : IGetEquipmentTypeByIdDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetEquipmentTypeByIdDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public async Task<EquipmentType> GetEquipmentTypeByIdAsync(string equipmentTypeId, CancellationToken cancellationToken)
    {
        var equipmentType = await _context
            .EquipmentTypes
            .AsNoTracking()
            .FirstAsync(x => x.Id == equipmentTypeId, cancellationToken);

        return new EquipmentType(
            equipmentType.Id,
            equipmentType.ShortName,
            equipmentType.FullName
        );
    }
}

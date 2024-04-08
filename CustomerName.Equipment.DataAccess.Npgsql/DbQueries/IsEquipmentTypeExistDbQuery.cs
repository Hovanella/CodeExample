using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsEquipmentTypeExistDbQuery : IIsEquipmentTypeExistDbQuery
{
    private readonly IEquipmentDbContext _context;

    public IsEquipmentTypeExistDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<bool> IsEquipmentTypeExist(string equipmentTypeId, CancellationToken cancellationToken)
    {
       return _context.EquipmentTypes
           .AsNoTracking()
           .AnyAsync(x => x.Id == equipmentTypeId, cancellationToken);
    }
}

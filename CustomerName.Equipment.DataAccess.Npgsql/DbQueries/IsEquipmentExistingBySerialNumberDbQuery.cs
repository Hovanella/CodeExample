using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsEquipmentExistingBySerialNumberDbQuery : IIsEquipmentExistingBySerialNumberDbQuery
{
    private readonly IEquipmentDbContext _context;

    public IsEquipmentExistingBySerialNumberDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<bool> IsEquipmentExistingBySerialNumberAsync(
        string serialNumber,
        CancellationToken cancellationToken)
    {
        return _context.Equipments
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.SerialNumber, serialNumber))
            .AnyAsync(cancellationToken);
    }
}

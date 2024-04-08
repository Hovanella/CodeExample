using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsEquipmentExistByIdDbQuery : IIsEquipmentExistByIdDbQuery
{
    private readonly IEquipmentDbContext _context;

    public IsEquipmentExistByIdDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExecuteAsync(int request, CancellationToken cancellationToken)
    {
        return _context
            .Equipments
            .AnyAsync(x => x.Id == request, cancellationToken);
    }
}

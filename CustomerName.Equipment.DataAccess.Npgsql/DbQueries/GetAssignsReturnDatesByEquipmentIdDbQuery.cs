using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetAssignsReturnDatesByEquipmentIdDbQuery : IGetAssignsReturnDatesByEquipmentIdDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetAssignsReturnDatesByEquipmentIdDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<DateTime?[]> ExecuteAsync(int equipmentId, CancellationToken cancellationToken)
    {
        return _context
            .EquipmentAssigns
            .Where(x => !x.IsDeleted && x.EquipmentId == equipmentId)
            .Select(x => x.ReturnDate)
            .ToArrayAsync(cancellationToken);
    }
}

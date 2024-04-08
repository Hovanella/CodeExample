using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetEquipmentTypesDbQuery : IGetEquipmentTypesDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetEquipmentTypesDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<List<EquipmentType>> GetEquipmentTypes(CancellationToken cancellationToken)
    {
        return _context
            .EquipmentTypes
            .AsNoTracking()
            .OrderBy(x=>x.Id)
            .Select(x => new EquipmentType(
                x.Id,
                x.ShortName,
                x.FullName
            ))
            .ToListAsync(cancellationToken);
    }
}

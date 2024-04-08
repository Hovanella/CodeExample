using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetEquipmentUsersLookup;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class GetEquipmentUsersLookupDbQuery : IGetEquipmentUsersLookupDbQuery
{
    private readonly IEquipmentDbContext _context;

    public GetEquipmentUsersLookupDbQuery(IEquipmentDbContext context)
    {
        _context = context;
    }

    public Task<List<EquipmentUserLookup>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return _context.EquipmentUsers
            .Where(x => x.IsActive)
            .OrderBy(x=>x.UserId)
            .Select(x => new EquipmentUserLookup(
                x.UserId,
                $"{x.LastName} {x.FirstName}"))
            .ToListAsync(cancellationToken: cancellationToken);
    }
}

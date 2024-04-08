using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsEquipmentUserExistDbQuery : IIsEquipmentUserExistDbQuery
{
    private readonly IEquipmentDbContext _dbContext;

    public IsEquipmentUserExistDbQuery(IEquipmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExecuteAsync(int request, CancellationToken cancellationToken)
    {
        return _dbContext.EquipmentUsers
               .AsNoTracking()
               .AnyAsync(entity => entity.UserId == request, cancellationToken);
    }
}

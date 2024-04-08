using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsUserApproverOfEquipmentDbQuery
    : IIsUserApproverOfEquipmentDbQuery
{
    private readonly IEquipmentDbContext _dbContext;

    public IsUserApproverOfEquipmentDbQuery(
        IEquipmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExecuteAsync(int userId, CancellationToken cancellationToken)
    {
        var hasActiveAssignments = await _dbContext.Equipments
            .AnyAsync(e => e.ApproverId == userId,
                cancellationToken);

        return hasActiveAssignments;
    }
}

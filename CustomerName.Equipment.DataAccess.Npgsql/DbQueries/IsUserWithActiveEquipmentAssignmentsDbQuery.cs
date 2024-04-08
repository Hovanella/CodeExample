using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsUserWithActiveEquipmentAssignmentsDbQuery : IIsUserWithActiveEquipmentAssignmentsDbQuery
{
    private readonly IEquipmentDbContext _dbContext;
    private readonly IClockService _clockService;

    public IsUserWithActiveEquipmentAssignmentsDbQuery(
        IEquipmentDbContext dbContext,
        IClockService clockService)
    {
        _dbContext = dbContext;
        _clockService = clockService;
    }

    public async Task<bool> ExecuteAsync(int userId, CancellationToken cancellationToken)
    {
        var currentDate = _clockService.UtcNow.Date;

        var hasActiveAssignments = await _dbContext.EquipmentAssigns
            .AsNoTracking()
            .AnyAsync(e =>
                !e.IsDeleted
                && e.AssignedToUserId == userId
                && (e.ReturnDate == null || e.ReturnDate.Value.Date > currentDate),
                cancellationToken);

        return hasActiveAssignments;
    }
}

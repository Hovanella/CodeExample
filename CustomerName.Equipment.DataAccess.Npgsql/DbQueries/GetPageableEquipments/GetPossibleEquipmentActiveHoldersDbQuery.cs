using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentActiveHolder;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.GetPageableEquipments;

internal class GetPossibleEquipmentActiveHoldersDbQuery : IGetPossibleEquipmentActiveHoldersDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;

    public GetPossibleEquipmentActiveHoldersDbQuery(IEquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public Task<List<FilterUser>> ExecuteAsync(GetPossibleEquipmentActiveHoldersDbQueryRequest request ,CancellationToken cancellationToken)
    {
        var activeUsers = _equipmentDbContext
            .EquipmentUsers
            .Where(x => x.IsActive);

        if (request is { IsAvailableEquipmentsWithoutAssigners: false, DepartmentId: not null })
        {
            activeUsers = activeUsers.Where(x => x.DepartmentId == request.DepartmentId);
        }

        return activeUsers
            .OrderBy(x=>x.UserId)
            .Select(x => new FilterUser(x.UserId, x.FirstName!, x.LastName!))
            .ToListAsync(cancellationToken);
    }
}

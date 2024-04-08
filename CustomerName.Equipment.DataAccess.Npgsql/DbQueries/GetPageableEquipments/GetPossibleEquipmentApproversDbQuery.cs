using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentApprovers;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.GetPageableEquipments;

internal class GetPossibleEquipmentApproversDbQuery : IGetPossibleEquipmentApproversDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;

    public GetPossibleEquipmentApproversDbQuery(IEquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public Task<List<FilterUser>> ExecuteAsync(List<int> managerIds, CancellationToken cancellationToken)
    {
        return _equipmentDbContext
            .EquipmentUsers
            .Where(x => x.IsActive && managerIds.Contains(x.UserId))
            .OrderBy(x => x.UserId)
            .Select(x => new FilterUser(x.UserId, x.FirstName!, x.LastName!))
            .ToListAsync(cancellationToken);
    }
}

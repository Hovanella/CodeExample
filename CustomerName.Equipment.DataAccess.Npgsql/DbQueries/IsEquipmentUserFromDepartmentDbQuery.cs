using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsEquipmentUserFromDepartment;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries;

internal class IsEquipmentUserFromDepartmentDbQuery : IIsEquipmentUserFromDepartmentDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;

    public IsEquipmentUserFromDepartmentDbQuery(IEquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public Task<bool> IsUserFromDepartment(int userId, string? departmentId, CancellationToken cancellationToken)
    {
        return _equipmentDbContext
            .EquipmentUsers
            .AnyAsync(x => x.UserId == userId && x.DepartmentId == departmentId,cancellationToken);
    }
}

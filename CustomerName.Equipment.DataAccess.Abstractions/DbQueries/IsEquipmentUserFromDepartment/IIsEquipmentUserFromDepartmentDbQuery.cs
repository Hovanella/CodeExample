namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsEquipmentUserFromDepartment;

public interface IIsEquipmentUserFromDepartmentDbQuery
{
    Task<bool> IsUserFromDepartment(int userId, string? departmentId, CancellationToken cancellationToken);
}

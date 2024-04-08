namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

public interface IIsEquipmentTypeExistDbQuery
{
    Task<bool> IsEquipmentTypeExist(string equipmentTypeId, CancellationToken cancellationToken);
}

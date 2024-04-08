namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

public interface IIsEquipmentExistingBySerialNumberDbQuery
{
    Task<bool> IsEquipmentExistingBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken);
}

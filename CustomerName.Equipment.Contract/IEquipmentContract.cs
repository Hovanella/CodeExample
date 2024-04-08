namespace CustomerName.Portal.Equipment.Contract;

public interface IEquipmentContract
{
    Task<bool> CanEquipmentUserBeDeactivated(int userId, CancellationToken cancellationToken);
}

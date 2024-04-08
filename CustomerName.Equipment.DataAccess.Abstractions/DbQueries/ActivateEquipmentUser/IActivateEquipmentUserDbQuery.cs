namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.ActivateEquipmentUser;

public interface IActivateEquipmentUserDbQuery
{
    Task ActivateUserAsync(ActivateEquipmentUserRequest request, CancellationToken cancellationToken);
}

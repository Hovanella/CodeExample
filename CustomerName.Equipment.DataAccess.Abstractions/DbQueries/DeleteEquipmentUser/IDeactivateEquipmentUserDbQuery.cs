namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.DeleteEquipmentUser;

public interface IDeactivateEquipmentUserDbQuery
{
    Task DeactivateUserAsync(DeactivateEquipmentUserDbQueryRequest request, CancellationToken cancellationToken);
}

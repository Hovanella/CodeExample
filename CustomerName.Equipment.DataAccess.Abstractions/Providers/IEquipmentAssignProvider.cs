using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;

internal interface IEquipmentAssignProvider
{
    Task<List<EquipmentAssign>> GetByEquipmentIdAsync(
        int equipmentId,
        CancellationToken cancellationToken);

    Task<List<EquipmentAssign>> GetEquipmentsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken);
}

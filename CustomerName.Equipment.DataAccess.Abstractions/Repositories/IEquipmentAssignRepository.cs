using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;

internal interface IEquipmentAssignRepository
{
    Task<EquipmentAssign> DeleteEquipmentAssignmentAsync(
        int assignId,
        CancellationToken cancellationToken);
}

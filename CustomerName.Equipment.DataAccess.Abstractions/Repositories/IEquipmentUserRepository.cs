using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;

public interface IEquipmentUserRepository
{
    Task CreateOrUpdateUserAsync(
        EquipmentUser equipmentUser,
        CancellationToken cancellationToken);
}

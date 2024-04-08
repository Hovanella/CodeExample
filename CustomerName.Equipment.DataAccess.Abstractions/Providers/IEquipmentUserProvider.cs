using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;

internal interface IEquipmentUserProvider
{
    Task<EquipmentUser?> GetUserByIdAsync(
        int userId,
        CancellationToken cancellationToken);
}

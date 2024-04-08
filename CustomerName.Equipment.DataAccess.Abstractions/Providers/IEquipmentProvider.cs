namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;

internal interface IEquipmentProvider
{
    Task<Domain.Equipment?> GetEquipmentByIdAsync(
        int equipmentId,
        CancellationToken cancellationToken);
}

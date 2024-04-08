using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

public interface IGetEquipmentTypeByIdDbQuery
{
    Task<EquipmentType> GetEquipmentTypeByIdAsync(
        string equipmentTypeId,
        CancellationToken cancellationToken);
}

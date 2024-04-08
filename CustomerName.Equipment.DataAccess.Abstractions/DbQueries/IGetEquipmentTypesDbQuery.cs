using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

public interface IGetEquipmentTypesDbQuery
{
    Task<List<EquipmentType>> GetEquipmentTypes(CancellationToken cancellationToken);
}

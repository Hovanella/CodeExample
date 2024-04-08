using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

internal interface IGetEquipmentReportByIdDbQuery
{
    Task<EquipmentReport?> GetEquipmentReportByIdAsync(
        int equipmentReportId,
        CancellationToken cancellationToken);
}

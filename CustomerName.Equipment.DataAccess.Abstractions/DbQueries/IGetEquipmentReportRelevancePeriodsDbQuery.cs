using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

internal interface IGetEquipmentReportRelevancePeriodsDbQuery
{
    Task<List<EquipmentReportRelevancePeriod>?> GetEquipmentReportRelevancePeriodsBySerialNumberAsync(
        string serialNumber,
        CancellationToken cancellationToken);
}

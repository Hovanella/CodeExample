namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

internal interface IUpdateEquipmentReportRelevancePeriodDbQuery
{
    Task UpdateEquipmentReportRelevancePeriodToUtcDateAsync(
        string serialNumber,
        int equipmentReportId,
        CancellationToken cancellationToken);
}

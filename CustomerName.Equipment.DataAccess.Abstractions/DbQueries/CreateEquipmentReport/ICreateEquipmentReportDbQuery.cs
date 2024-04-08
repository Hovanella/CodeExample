using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentReport;

internal interface ICreateEquipmentReportDbQuery
{
    Task<EquipmentReport> CreateEquipmentReportAsync(
        CreateEquipmentReportDbQueryRequest request,
        CancellationToken cancellationToken);
}

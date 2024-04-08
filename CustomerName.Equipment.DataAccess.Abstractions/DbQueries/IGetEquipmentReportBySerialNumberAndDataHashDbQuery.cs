using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

internal interface IGetEquipmentReportBySerialNumberAndDataHashDbQuery
{
    Task<EquipmentReport?> GetEquipmentReportBySerialNumberAndDataHashAsync(
        string serialNumber,
        string dataHash,
        CancellationToken cancellationToken);
}

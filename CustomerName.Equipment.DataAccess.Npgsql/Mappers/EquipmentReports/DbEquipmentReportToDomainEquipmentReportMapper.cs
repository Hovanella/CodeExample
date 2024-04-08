using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;

internal interface IDbEquipmentReportToDomainEquipmentReportMapper
{
    EquipmentReport? Map(DatabaseEntities.EquipmentReport? equipmentReport);
}

internal class DbEquipmentReportToDomainEquipmentReportMapper :
    IDbEquipmentReportToDomainEquipmentReportMapper
{
    public EquipmentReport? Map(DatabaseEntities.EquipmentReport? equipmentReport)
    {
        return equipmentReport is null ?  null : new EquipmentReport
        {
            Id = equipmentReport.Id,
            SerialNumber = equipmentReport.SerialNumber,
            Data = equipmentReport.Data,
            DataHash = equipmentReport.DataHash,
            AssembledAtUtc = equipmentReport.AssembledAtUtc,
            CreatedAtUtc = equipmentReport.CreatedAtUtc
        };
    }
}
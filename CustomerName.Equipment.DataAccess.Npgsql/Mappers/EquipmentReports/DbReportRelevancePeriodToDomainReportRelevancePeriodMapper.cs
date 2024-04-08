using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;

internal interface IDbReportRelevancePeriodToDomainReportRelevancePeriodMapper
{
    EquipmentReportRelevancePeriod Map(DatabaseEntities.EquipmentReportRelevancePeriod equipmentReportDto);
    IEnumerable<EquipmentReportRelevancePeriod> Map(List<DatabaseEntities.EquipmentReportRelevancePeriod> reportRelevancePeriods);
}

internal class DbReportRelevancePeriodToDomainReportRelevancePeriodMapper :
    IDbReportRelevancePeriodToDomainReportRelevancePeriodMapper
{
    public EquipmentReportRelevancePeriod Map(DatabaseEntities.EquipmentReportRelevancePeriod equipmentReportDto)
    {
        return new EquipmentReportRelevancePeriod
        {
            EquipmentReportId = equipmentReportDto.EquipmentReportId,
            FromUtc = equipmentReportDto.FromUtc,
            ToUtc = equipmentReportDto.ToUtc
        };
    }

    public IEnumerable<EquipmentReportRelevancePeriod> Map(List<DatabaseEntities.EquipmentReportRelevancePeriod> reportRelevancePeriods)
    {
        foreach (var item in reportRelevancePeriods)
        {
            yield return Map(item);
        }
    }
}

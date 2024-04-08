using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;

internal interface IReportRelevancePeriodToReportRelevancePeriodDtoMapper
{
    EquipmentReportRelevancePeriodDto Map(EquipmentReportRelevancePeriod reportRelevancePeriod);
    IEnumerable<EquipmentReportRelevancePeriodDto> Map(List<EquipmentReportRelevancePeriod> reportRelevancePeriods);
}

internal class ReportRelevancePeriodToReportRelevancePeriodDtoMapper :
    IReportRelevancePeriodToReportRelevancePeriodDtoMapper
{
    public EquipmentReportRelevancePeriodDto Map(EquipmentReportRelevancePeriod reportRelevancePeriod)
    {
        return new EquipmentReportRelevancePeriodDto
        {
            EquipmentReportId = reportRelevancePeriod.EquipmentReportId,
            FromUtc = reportRelevancePeriod.FromUtc,
            ToUtc = reportRelevancePeriod.ToUtc
        };
    }

    public IEnumerable<EquipmentReportRelevancePeriodDto> Map(List<EquipmentReportRelevancePeriod> reportRelevancePeriods)
    {
        foreach (var item in reportRelevancePeriods)
        {
            yield return Map(item);
        }
    }
}

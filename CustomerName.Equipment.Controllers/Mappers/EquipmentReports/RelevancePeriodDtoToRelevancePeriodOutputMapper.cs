using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.Controllers.Mappers.EquipmentReports;

internal interface IRelevancePeriodDtoToRelevancePeriodOutputMapper
{
    EquipmentReportRelevancePeriodOutput Map(EquipmentReportRelevancePeriodDto equipmentReportDto);
    IEnumerable<EquipmentReportRelevancePeriodOutput> Map(List<EquipmentReportRelevancePeriodDto> equipmentReportDtos);
}

internal class RelevancePeriodDtoToRelevancePeriodOutputMapper :
    IRelevancePeriodDtoToRelevancePeriodOutputMapper
{
    public EquipmentReportRelevancePeriodOutput Map(EquipmentReportRelevancePeriodDto equipmentReportDto)
    {
        return new EquipmentReportRelevancePeriodOutput
        {
            EquipmentReportId = equipmentReportDto.EquipmentReportId,
            FromUtc = equipmentReportDto.FromUtc,
            ToUtc = equipmentReportDto.ToUtc
        };
    }

    public IEnumerable<EquipmentReportRelevancePeriodOutput> Map(List<EquipmentReportRelevancePeriodDto> equipmentReportDtos)
    {
        foreach (var item in equipmentReportDtos)
        {
            yield return Map(item);
        }
    }
}
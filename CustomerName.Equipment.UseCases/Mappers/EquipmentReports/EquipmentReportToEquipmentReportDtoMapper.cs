using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;

internal interface IEquipmentReportToEquipmentReportDtoMapper
{
    EquipmentReportDto Map(EquipmentReport equipmentReport);
}

internal class EquipmentReportToEquipmentReportDtoMapper :
    IEquipmentReportToEquipmentReportDtoMapper
{
    public EquipmentReportDto Map(EquipmentReport equipmentReport)
    {
        return new EquipmentReportDto
        {
            Id = equipmentReport.Id,
            SerialNumber = equipmentReport.SerialNumber,
            Data = equipmentReport.Data,
            AssembledAtUtc = equipmentReport.AssembledAtUtc,
            CreatedAtUtc = equipmentReport.CreatedAtUtc
        };
    }
}
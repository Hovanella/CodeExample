using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.UseCases.Dto;

namespace CustomerName.Portal.Equipment.Controllers.Mappers.EquipmentReports;

internal interface IEquipmentReportDtoToEquipmentReportOutputMapper
{
    EquipmentReportOutput Map(EquipmentReportDto equipmentReportDto);
}

internal class EquipmentReportDtoToEquipmentReportOutputMapper :
    IEquipmentReportDtoToEquipmentReportOutputMapper
{
    public EquipmentReportOutput Map(EquipmentReportDto equipmentReportDto)
    {
        return new EquipmentReportOutput
        {
            Id = equipmentReportDto.Id,
            Data = equipmentReportDto.Data,
            SerialNumber = equipmentReportDto.SerialNumber,
            AssembledAtUtc = equipmentReportDto.AssembledAtUtc,
            CreatedAtUtc = equipmentReportDto.CreatedAtUtc
        };
    }
}

namespace CustomerName.Portal.Equipment.UseCases.Dto;

internal class EquipmentReportRelevancePeriodDto
{
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public int EquipmentReportId { get; set; }
}

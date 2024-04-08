namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

public class EquipmentReportRelevancePeriodOutput
{
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public int EquipmentReportId { get; set; }
}

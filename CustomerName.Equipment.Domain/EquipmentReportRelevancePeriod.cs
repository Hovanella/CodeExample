namespace CustomerName.Portal.Equipment.Domain;

internal class EquipmentReportRelevancePeriod
{
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public int EquipmentReportId { get; set; }

    public EquipmentReport? EquipmentReport { get; set; }
}

namespace CustomerName.Portal.Equipment.Domain;

internal class EquipmentReport
{
    public int Id { get; set; }
    public required string SerialNumber { get; set; }
    public required string Data { get; set; }
    public required string DataHash { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime AssembledAtUtc { get; set; }

    public List<EquipmentReportRelevancePeriod> EquipmentReportRelevancePeriods { get; set; } = [];
}

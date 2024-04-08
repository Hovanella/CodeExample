namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

internal class EquipmentReportRelevancePeriod
{
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
    public int EquipmentReportId { get; set; }

    public EquipmentReport? EquipmentReport { get; set; }
}

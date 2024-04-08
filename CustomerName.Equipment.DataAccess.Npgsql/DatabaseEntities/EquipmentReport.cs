namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

internal class EquipmentReport
{
    public int Id { get; set; }
    public required string SerialNumber { get; set; }
    public required string Data { get; set; }
    public required string DataHash { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime AssembledAtUtc { get; set; }

    public ICollection<EquipmentReportRelevancePeriod> EquipmentReportRelevancePeriods { get; set; } = new List<EquipmentReportRelevancePeriod>();
}

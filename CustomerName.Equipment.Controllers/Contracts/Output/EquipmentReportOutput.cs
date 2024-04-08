namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

public class EquipmentReportOutput
{
    public int Id { get; set; }
    public required string SerialNumber { get; set; }
    public required string Data { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime AssembledAtUtc { get; set; }
}

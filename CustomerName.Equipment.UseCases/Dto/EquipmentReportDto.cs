namespace CustomerName.Portal.Equipment.UseCases.Dto;

internal class EquipmentReportDto
{
    public int Id { get; set; }
    public required string SerialNumber { get; set; }
    public required string Data { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime AssembledAtUtc { get; set; }
}

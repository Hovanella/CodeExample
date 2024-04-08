namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

public class UserEquipmentOutput
{
    public required string EquipmentTypeId { get; set; }
    public required string EquipmentName { get; set; }
    public required string SerialNumber { get; set; }
    public required string IssueDate { get; set; }
    public string? ReturnDate { get; set; }
}

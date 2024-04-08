namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

public class EquipmentHolderOutput
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public string? DepartmentId { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}

namespace CustomerName.Portal.Equipment.UseCases.Dto;

internal class UserEquipmentDto
{
    public string? EquipmentTypeId { get; set; }
    public required string EquipmentName { get; set; }
    public required string SerialNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}

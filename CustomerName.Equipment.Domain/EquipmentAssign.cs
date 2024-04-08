namespace CustomerName.Portal.Equipment.Domain;

internal class EquipmentAssign
{
    public int Id { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsDeleted { get; set; }

    public int AssignedToUserId { get; set; }
    public EquipmentUser? User { get; set; }

    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
}

using CustomerName.Portal.Framework.Entities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

internal class EquipmentAssign : AuditableEntity
{
    public int Id { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public int AssignedToUserId { get; set; }
    public EquipmentUser? User { get; set; }

    public int EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }

    public bool IsDeleted { get; set; }
}

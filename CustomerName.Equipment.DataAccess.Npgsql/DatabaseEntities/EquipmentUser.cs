using CustomerName.Portal.Framework.Entities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

internal class EquipmentUser : AuditableEntity
{
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? DepartmentId { get; set; }
    public bool IsActive { get; set; }
    public ICollection<EquipmentAssign> EquipmentAssigns { get; set; } = new List<EquipmentAssign>();
    public ICollection<Equipment> ApprovedEquipments { get; set; } = new List<Equipment>();
}

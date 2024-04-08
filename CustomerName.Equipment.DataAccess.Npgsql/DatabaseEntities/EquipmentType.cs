using CustomerName.Portal.Framework.Entities;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;

public class EquipmentType : AuditableEntity
{
    public string Id { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string FullName { get; set; } = null!;
}

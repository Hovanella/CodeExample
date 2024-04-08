using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.Controllers.Contracts.Input;

internal class GetEquipmentsByFileInput
{
    public required DocumentType Format { get; set; }
    public string? SearchPattern { get; set; }
    public bool? IsAvailable { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? TypeId { get; set; }
    public int? ApproverId { get; set; }
    public EquipmentLocationType? Location { get; set; }
    public DateTime? PurchaseDateFrom { get; set; }
    public DateTime? PurchaseDateTo { get; set; }
    public OrderByEquipments? OrderBy { get; set; }
    public bool? OrderByDesc { get; set; }
}

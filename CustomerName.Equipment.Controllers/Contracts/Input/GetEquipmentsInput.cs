using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.Controllers.Contracts.Input;

public class GetEquipmentsInput
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchPattern { get; set; }
    public bool? IsAvailable { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? EquipmentTypeId { get; set; }
    public int? ApproverId { get; set; }
    public EquipmentLocationType? Location { get; set; }
    public DateTime? PurchaseDateFrom { get; set; }
    public DateTime? PurchaseDateTo { get; set; }
    public OrderByEquipments? OrderBy { get; set; }
    public bool? OrderByDesc { get; set; }
}

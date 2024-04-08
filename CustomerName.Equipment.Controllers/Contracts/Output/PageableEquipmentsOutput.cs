using CustomerName.Portal.Framework.Utils;

namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

public class PageableEquipmentsOutput : PageableListOfItems<EquipmentOutput>
{
    /// <summary>
    /// Filters values that can be applied to the returned EquipmentOutput items
    /// </summary>
    public required AvailableFilterOptionsOutput FilterValues { get; set; }
}

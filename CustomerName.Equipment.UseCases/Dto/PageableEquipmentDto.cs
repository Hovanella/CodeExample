using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Equipment.UseCases.Dto.FilterOptions;
using CustomerName.Portal.Framework.Utils;

namespace CustomerName.Portal.Equipment.UseCases.Dto;

internal class PageableEquipmentDto : PageableListOfItems<OdpEquipment>
{
    public required AvailableFilterOptionsDto FilterValues { get; set; }
}

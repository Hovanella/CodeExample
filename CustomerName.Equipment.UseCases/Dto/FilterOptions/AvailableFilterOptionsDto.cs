using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.UseCases.Dto.FilterOptions;

internal class AvailableFilterOptionsDto
{
    public bool HasAvailable { get; set; }
    public required List<string> EquipmentTypeIds { get; set; }
    public required List<EquipmentLocationType> EquipmentLocations { get; set; }
    public required List<FilterUser> Approvers { get; set; }
    public required List<FilterUser> Users { get; set; }
}

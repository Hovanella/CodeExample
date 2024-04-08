namespace CustomerName.Portal.Equipment.Controllers.Contracts.Output;

public class AvailableFilterOptionsOutput
{
    public bool HasAvailable { get; set; }
    public required List<string> EquipmentTypeIds { get; set; }
    public required List<int> EquipmentLocations { get; set; }
    public required List<UserFilterOutput> Approvers { get; set; }
    public required List<UserFilterOutput> Users { get; set; }
}

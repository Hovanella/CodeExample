using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;
using CustomerName.Portal.Migrator.Contract;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Seeds;

internal class EquipmentTypeSeed : ISeedEntitiesProvider<EquipmentDbContext>
{
    private readonly EquipmentDbContext _dbContext;
    private readonly IClockService _clock;

    public EquipmentTypeSeed(EquipmentDbContext dbContext, IClockService clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }

    public void Seed()
    {
        foreach (var equipmentType in GetEquipmentTypes())
        {
            CreateIfNotExists(equipmentType);
        }

        _dbContext.SaveChanges();
    }

    private void CreateIfNotExists(EquipmentType equipmentType)
    {
        var hasAny = _dbContext.Set<EquipmentType>().Any(x => x.Id == equipmentType.Id);

        if (!hasAny)
        {
            _dbContext.Set<EquipmentType>().Add(equipmentType);
        }
    }

    private IEnumerable<EquipmentType> GetEquipmentTypes()
    {
        yield return Create("DesktopComputer", "DesktopComputer", "Desktop computer");
        yield return Create("Laptop", "Laptop", "Laptop");
        yield return Create("MacBook", "MacBook", "MacBook");
        yield return Create("MacBookMini", "MacBookMini", "MacBook mini");
        yield return Create("MobilePhone", "MobilePhone", "Mobile phone");
        yield return Create("Monitor", "Monitor", "Monitor");
        yield return Create("Keyboard", "Keyboard", "Keyboard");
        yield return Create("Mouse", "Mouse", "Mouse");
        yield return Create("MouseWithKeyboard", "MouseWithKeyboard", "Mouse + keyboard");
        yield return Create("Headset", "Headset", "Headset");
        yield return Create("WebCamera", "WebCamera", "Web camera");
        yield return Create("Firewall", "Firewall", "Firewall");
        yield return Create("AccessPoint", "AccessPoint", "Access point");
        yield return Create("Switch", "Switch", "Switch");
        yield return Create("Router", "Router", "Router");
        yield return Create("NetworkAdapter", "NetworkAdapter", "Network adapter");
        yield return Create("Printer", "Printer", "Printer");
        yield return Create("FlashDrive", "FlashDrive", "Flash drive");
        yield return Create("ExternalHardDrive", "ExternalHardDrive", "External hard drive");
        yield return Create("MultiAdapter", "MultiAdapter", "Multi-adapter");
        yield return Create("PcAccessoriesHdd", "PcAccessoriesHdd", "PC accessory: HDD");
        yield return Create("PcAccessoriesSsd", "PcAccessoriesSsd", "PC accessory: SSD");
        yield return Create("PcAccessoriesRam", "PcAccessoriesRam", "PC accessory: RAM");

        EquipmentType Create(string id, string shortName, string fullname)
        {
            return new EquipmentType
            {
                Id = id,
                ShortName = shortName,
                FullName = fullname,
                CreatedAtUtc = _clock.UtcNow,
                CreatedById = PortalConstants.SystemUserId
            };
        }
    }
}

using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;
using CustomerName.Portal.Migrator.Contract;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Seeds;

internal class EquipmentUserSeed : ISeedEntitiesProvider<EquipmentDbContext>
{
    private readonly EquipmentDbContext _dbContext;
    private readonly IClockService _clock;

    public EquipmentUserSeed(EquipmentDbContext dbContext, IClockService clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }

    public void Seed()
    {
        if (!_dbContext.EquipmentUsers.Any(x => x.Email == "CustomerNameCompany@CustomerName-software.com"))
        {
            var equipmentUsers = _dbContext.Set<EquipmentUser>();

            var CustomerNameCompanyEquipmentUser = new EquipmentUser
            {
                UserId = 0,
                IsActive = true,
                FirstName = "Company",
                LastName = "CustomerName",
                Email = "CustomerNameCompany@CustomerName-software.com",
                DepartmentId = null,
                CreatedAtUtc = _clock.UtcNow,
                CreatedById = PortalConstants.SystemUserId
            };

            equipmentUsers.Add(CustomerNameCompanyEquipmentUser);

            _dbContext.SaveChanges();
        }
    }
}

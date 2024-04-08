using Bogus;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;
using CustomerName.Portal.Migrator.Contract;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Seeds;

internal class EquipmentAssignSeed : ISeedEntitiesProvider<EquipmentDbContext>
{
    private readonly EquipmentDbContext _dbContext;
    private readonly IClockService _clock;
    private readonly IHostEnvironmentContext _hostEnvironmentContext;

    public EquipmentAssignSeed(
        EquipmentDbContext dbContext,
        IClockService clock,
        IHostEnvironmentContext hostEnvironmentContext)
    {
        _dbContext = dbContext;
        _clock = clock;
        _hostEnvironmentContext = hostEnvironmentContext;
    }

    private int FakeEquipmentAssignsCount => _hostEnvironmentContext.IsFunctionalTestEnvironment() || _hostEnvironmentContext.IsIntegrationTestEnvironment()
        ? 10
        : 40;

    public void Seed()
    {
        if (_hostEnvironmentContext.IsFunctionalTestEnvironment() ||
            _hostEnvironmentContext.IsDevelopmentEnvironment() ||
            _hostEnvironmentContext.IsQaEnvironment())
        {
            var equipmentAssigns = _dbContext.Set<EquipmentAssign>();

            if (equipmentAssigns.Count() < FakeEquipmentAssignsCount)
            {
                var equipments = _dbContext
                    .Set<DatabaseEntities.Equipment>()
                    .OrderBy(equipment => equipment.Id)
                    .Take(FakeEquipmentAssignsCount)
                    .ToList();

                var userIds = _dbContext
                    .Set<EquipmentUser>()
                    .OrderBy(x => x.UserId)
                    .Select(x => x.UserId)
                    .Take(150)
                    .ToArray();

                var i = 0;
                var faker = new Faker<EquipmentAssign>()
                    .RuleFor(
                        assign => assign.AssignedToUserId,
                        faker => faker.Random.ArrayElement(userIds))
                    .RuleFor(
                        assign => assign.IssueDate,
                        faker => faker.Date.Past(1, _clock.UtcNow.AddYears(-2)).Date.ToUniversalTime())
                    .RuleFor(
                        assign => assign.ReturnDate,
                        faker =>
                        {
                            i++;

                            return i % 3 == 0
                                ? faker.Date.Past(1, _clock.UtcNow).Date.ToUniversalTime()
                                : i % 5 == 0
                                    ? faker.Date.Future(1, _clock.UtcNow).Date.ToUniversalTime()
                                    : null;
                        });

                equipments.ForEach(e => e.Assigns.Add(faker.Generate()));

                _dbContext.SaveChanges();
            }
        }
    }
}

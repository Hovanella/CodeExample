using Bogus;
using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DatabaseEntities;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;
using CustomerName.Portal.Migrator.Contract;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Seeds;

internal class EquipmentSeed : ISeedEntitiesProvider<EquipmentDbContext>
{
    private readonly EquipmentDbContext _dbContext;
    private readonly IClockService _clock;
    private readonly IHostEnvironmentContext _hostEnvironmentContext;

    public EquipmentSeed(
        EquipmentDbContext dbContext,
        IClockService clock,
        IHostEnvironmentContext hostEnvironmentContext)
    {
        _dbContext = dbContext;
        _clock = clock;
        _hostEnvironmentContext = hostEnvironmentContext;
    }

    private int FakeEquipmentsCount =>
        _hostEnvironmentContext.IsFunctionalTestEnvironment() || _hostEnvironmentContext.IsIntegrationTestEnvironment()
        ? 3
        : 50;

    public void Seed()
    {
        if (_hostEnvironmentContext.IsDevelopmentEnvironment() ||
            _hostEnvironmentContext.IsQaEnvironment() ||
            _hostEnvironmentContext.IsIntegrationTestEnvironment() ||
            _hostEnvironmentContext.IsFunctionalTestEnvironment()
       )
        {
            var equipments = _dbContext.Set<DatabaseEntities.Equipment>();
            var equipmentUsers = _dbContext.Set<EquipmentUser>();

            if (equipments.Count() < FakeEquipmentsCount)
            {
                var ceoId = GetUserIdByEmail(
                    equipmentUsers,
                    "CustomerName.portal.test.ceo@CustomerName-software.com");
                var ctoId = GetUserIdByEmail(
                    equipmentUsers,
                    "CustomerName.portal.test.cto@CustomerName-software.com");
                var headId = GetUserIdByEmail(
                    equipmentUsers,
                    "CustomerName.portal.test.headofdepartment@CustomerName-software.com");

                var approverIds = new[] { ceoId, ctoId, headId }
                    .Where(id => id.HasValue)
                    .Select(id => id!.Value)
                    .ToArray();

                var equipmentTypeIds = _dbContext
                    .EquipmentTypes
                    .Select(x => x.Id)
                    .ToArray();

                var faker = new Faker<DatabaseEntities.Equipment>()
                    .RuleFor(x => x.Name, x => x.Commerce.ProductName())
                    .RuleFor(x => x.TypeId, x => x.Random.ArrayElement(equipmentTypeIds))
                    .RuleFor(x => x.Location, x => x.Random.Enum<EquipmentLocationType>())
                    .RuleFor(x => x.SerialNumber, x => x.Random.AlphaNumeric(9))
                    .RuleFor(x => x.PurchasePrice, x => decimal.Round(x.Random.Decimal(100, 1000), 2))
                    .RuleFor(x => x.PurchasePriceUsd, x => decimal.Round(x.Random.Decimal(10, 100), 2))
                    .RuleFor(x => x.PurchaseCurrency, x => x.Random.Enum<MoneyCurrencyType>())
                    .RuleFor(x => x.PurchaseDate, x => x.Date.Past(3, _clock.UtcNow).Date.ToUniversalTime())
                    .RuleFor(x => x.PurchasePlace, x => x.Address.City())
                    .RuleFor(x => x.GuaranteeDate, x => x.Date.Future(3, _clock.UtcNow).Date.ToUniversalTime())
                    .RuleFor(x => x.Characteristics, x => x.Lorem.Text())
                    .RuleFor(x => x.Comment, x => x.Lorem.Text())
                    .RuleFor(x => x.ApproverId, x =>
                        approverIds.Length == 0
                            ? null
                            : x.Random.ArrayElement(approverIds));

                equipments.AddRange(faker.Generate(FakeEquipmentsCount));

                _dbContext.SaveChanges();
            }
        }
    }

    private static int? GetUserIdByEmail(DbSet<EquipmentUser> equipmentUsers, string emailAddress)
    {
        return equipmentUsers
            .Where(x => x.Email == emailAddress)
            .Select(x => new { x.UserId })
            .FirstOrDefault()
            ?.UserId;
    }
}

using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Migrator.Contract;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Seeds;

internal static class SeedDataExtensions
{
    internal static IServiceCollection AddSeeds(this IServiceCollection services)
    {
        services.AddTransient<ISeedEntitiesProvider<EquipmentDbContext>, EquipmentTypeSeed>();
        services.AddTransient<ISeedEntitiesProvider<EquipmentDbContext>, EquipmentSeed>();
        services.AddTransient<ISeedEntitiesProvider<EquipmentDbContext>, EquipmentAssignSeed>();
        services.AddTransient<ISeedEntitiesProvider<EquipmentDbContext>, EquipmentUserSeed>();

        return services;
    }
}

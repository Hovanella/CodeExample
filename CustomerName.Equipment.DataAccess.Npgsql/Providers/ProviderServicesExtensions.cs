using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Providers;

internal static class ProviderServicesExtensions
{
    internal static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IEquipmentProvider, EquipmentProvider>();
        services.AddScoped<IEquipmentAssignProvider, EquipmentAssignProvider>();
        services.AddScoped<IEquipmentUserProvider, EquipmentUserProvider>();
        services.AddScoped<IEquipmentAssignProvider, EquipmentAssignProvider>();

        return services;
    }
}

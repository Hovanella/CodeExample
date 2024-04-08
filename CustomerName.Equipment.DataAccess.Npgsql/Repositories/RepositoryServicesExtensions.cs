using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Repositories;

internal static class RepositoryServicesExtensions
{
    internal static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEquipmentUserRepository, EquipmentUserRepository>();
        services.AddScoped<IEquipmentAssignRepository, EquipmentAssignRepository>();

        return services;
    }
}

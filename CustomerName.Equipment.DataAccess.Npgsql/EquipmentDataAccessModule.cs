using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.Extensions;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Providers;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Repositories;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Seeds;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;
using CustomerName.Portal.Framework.Utils.Modules;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql;

public class EquipmentDataAccessModule : Module
{
    public override void Load(IServiceCollection services)
    {
        services.AddDbContext<EquipmentDbContext>((serviceProvider, optionsBuilder) =>
        {
            var factory = serviceProvider.GetRequiredService<IConnectionFactory>();

            optionsBuilder.UseNpgsql(factory.GetConnection(), pgOptions =>
            {
                pgOptions.MigrationsHistoryTable(
                    HistoryRepository.DefaultTableName,
                    EquipmentDbContext.Schema);
            })
            .AddInterceptors(serviceProvider.GetRequiredService<ISaveChangesInterceptor>());
        });

        services.AddScoped<IEquipmentDbContext>(serviceProvider =>
        {
            var context = serviceProvider.GetRequiredService<EquipmentDbContext>();
            var connectionFactory = serviceProvider.GetRequiredService<IConnectionFactory>();

            context.Database.UseTransaction(connectionFactory.GetTransaction());

            return context;
        });

        services.AddDbQueries();
        services.AddRepositories();
        services.AddProviders();
        services.AddDbMappers();
        services.AddSeeds();
    }
}

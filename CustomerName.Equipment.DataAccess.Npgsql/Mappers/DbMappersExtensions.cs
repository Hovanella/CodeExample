using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers.EquipmentReports;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.Mappers;

internal static class DbMappersExtensions
{
    internal static IServiceCollection AddDbMappers(this IServiceCollection services)
    {
        services.AddScoped<ICreateEquipmentReportDbQueryRequestToDbEquipmentReportMapper,
                            CreateEquipmentReportDbQueryRequestToDbEquipmentReportMapper>();
        services.AddScoped<IDbEquipmentReportToDomainEquipmentReportMapper,
                            DbEquipmentReportToDomainEquipmentReportMapper>();
        services.AddScoped<IDbReportRelevancePeriodToDomainReportRelevancePeriodMapper,
                            DbReportRelevancePeriodToDomainReportRelevancePeriodMapper>();

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentExports;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;

namespace CustomerName.Portal.Equipment.UseCases.Mappers;

internal static class UseCasesMappersExtensions
{
    internal static IServiceCollection AddUseCaseMappers(this IServiceCollection services)
    {
        services
            .AddScoped<IEquipmentReportToEquipmentReportDtoMapper,
                EquipmentReportToEquipmentReportDtoMapper>()
            .AddScoped<IReportRelevancePeriodToReportRelevancePeriodDtoMapper,
                ReportRelevancePeriodToReportRelevancePeriodDtoMapper>()
            .AddScoped<IUpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper,
                UpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper>()
            .AddScoped<IUpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper,
                UpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper>()
            .AddScoped<IUpdateEquipmentCommandToEquipmentMapper,
                UpdateEquipmentCommandToEquipmentMapper>();

        services.AddScoped<ICreateEquipmentCommandToEquipmentMapper,
                            CreateEquipmentCommandToEquipmentMapper>();
        services.AddScoped<IEquipmentToEquipmentDtoMapper,
                            EquipmentToEquipmentDtoMapper>();

        services.AddScoped<ICreateEquipmentCommandToEquipmentMapper,
                            CreateEquipmentCommandToEquipmentMapper>();
        services.AddScoped<IEquipmentToEquipmentDtoMapper,
                            EquipmentToEquipmentDtoMapper>();

        services.AddScoped<IOdpEquipmentToEquipmentToCsvExportMapper,
                            OdpEquipmentToEquipmentToCsvExportMapper>();
        services.AddScoped<IOdpEquipmentToEquipmentToExcelExportMapper,
                             OdpEquipmentToEquipmentToExcelExportMapper>();

        return services;
    }
}

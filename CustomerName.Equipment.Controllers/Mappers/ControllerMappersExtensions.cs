using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.Controllers.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.Controllers.Mappers.PageableEquipments;

namespace CustomerName.Portal.Equipment.Controllers.Mappers;

internal static class ControllerMappersExtensions
{
    internal static IServiceCollection AddControllerMappers(this IServiceCollection services)
    {
        services
            .AddScoped<IEquipmentReportDtoToEquipmentReportOutputMapper,
                EquipmentReportDtoToEquipmentReportOutputMapper>()
            .AddScoped<IRelevancePeriodDtoToRelevancePeriodOutputMapper,
                RelevancePeriodDtoToRelevancePeriodOutputMapper>()
            .AddScoped<IActiveHolderDtoToEquipmentHolderOutputMapper,
                ActiveHolderDtoToEquipmentHolderOutputMapper>()
            .AddScoped<IUpdatedEquipmentDtoToUpdatedEquipmentOutputMapper,
                UpdatedEquipmentDtoToUpdatedEquipmentOutputMapper>()
            .AddScoped<IPageableEquipmentDtoToPageableEquipmentsOutputMapper,
                PageableEquipmentDtoToPageableEquipmentsOutputMapper>()
            .AddScoped<IEquipmentDtoToEquipmentOutputMapper,
                EquipmentDtoToEquipmentOutputMapper>();

        return services;
    }
}

using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.Controllers.Mappers;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Framework.Utils;
using CustomerName.Portal.Framework.Utils.Modules;

namespace CustomerName.Portal.Equipment.Controllers;

public class EquipmentControllersModule : Module
{
    public override void Load(IServiceCollection services)
    {
        services.AddControllers()
            .AddOData(setup =>
            {
                setup.TimeZone = TimeZoneInfo.Utc;

                setup
                    .Filter()
                    .OrderBy()
                    .SetMaxTop(100)
                    .AddRouteComponents(
                        "api/equipment/odata",
                        EdmModelBuilder.BuildWithoutExtraConfiguration<OdpEquipment>());
            });

        services.AddControllerMappers();
    }
}

using Microsoft.Extensions.DependencyInjection;
using CustomerName.Portal.Equipment.Contract;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Equipment.UseCases.Mappers;
using CustomerName.Portal.Framework.Utils.Modules;

namespace CustomerName.Portal.Equipment.UseCases;

public class EquipmentUseCasesModule : Module
{
    public override void Load(IServiceCollection services)
    {
        AddBusinessRules(services);
        services.AddUseCaseMappers();
        
        services.AddScoped<IEquipmentContract, EquipmentContract>();
    }

    private void AddBusinessRules(IServiceCollection services)
    {
        services.AddScoped<ICanEquipmentUserBeDeactivatedBusinessRule, CanEquipmentUserBeDeactivatedBusinessRule>();
        services.AddScoped<IIsAvailableEquipmentsWithoutAssignersBusinessRule,IsAvailableEquipmentsWithoutAssignersBusinessRule>();
        services.AddScoped<IIsAllowedToGetUserEquipmentBusinessRule, IsAllowedToGetUserEquipmentBusinessRule>();
    }
}

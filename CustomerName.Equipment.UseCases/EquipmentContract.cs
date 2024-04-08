using CustomerName.Portal.Equipment.Contract;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;

namespace CustomerName.Portal.Equipment.UseCases;

internal class EquipmentContract : IEquipmentContract
{
    private readonly ICanEquipmentUserBeDeactivatedBusinessRule _canEquipmentUserBeDeactivatedBusinessRule;

    public EquipmentContract(ICanEquipmentUserBeDeactivatedBusinessRule canEquipmentUserBeDeactivatedBusinessRule)
    {
        _canEquipmentUserBeDeactivatedBusinessRule = canEquipmentUserBeDeactivatedBusinessRule;
    }

    public Task<bool> CanEquipmentUserBeDeactivated(int userId, CancellationToken cancellationToken)
    {
        return _canEquipmentUserBeDeactivatedBusinessRule.IsCorrespondsToBusiness(userId, cancellationToken);
    }
}

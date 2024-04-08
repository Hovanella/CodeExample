using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.UseCases.BusinessRules;

internal interface ICanEquipmentUserBeDeactivatedBusinessRule
{
    Task<bool> IsCorrespondsToBusiness(int userId, CancellationToken cancellationToken);
}

internal class CanEquipmentUserBeDeactivatedBusinessRule : ICanEquipmentUserBeDeactivatedBusinessRule
{
    private readonly IIsUserWithActiveEquipmentAssignmentsDbQuery _isUserWithActiveEquipmentAssignmentsDbQuery;
    private readonly IIsUserApproverOfEquipmentDbQuery _isUserApproverOfEquipmentDbQuery;

    public CanEquipmentUserBeDeactivatedBusinessRule(
        IIsUserWithActiveEquipmentAssignmentsDbQuery isUserWithActiveEquipmentAssignmentsDbQuery,
        IIsUserApproverOfEquipmentDbQuery isUserApproverOfEquipmentDbQuery)
    {
        _isUserWithActiveEquipmentAssignmentsDbQuery = isUserWithActiveEquipmentAssignmentsDbQuery;
        _isUserApproverOfEquipmentDbQuery = isUserApproverOfEquipmentDbQuery;
    }

    public async Task<bool> IsCorrespondsToBusiness(int userId, CancellationToken cancellationToken)
    {
        var hasActiveAssignments = await _isUserWithActiveEquipmentAssignmentsDbQuery
            .ExecuteAsync(userId, cancellationToken);

        var isUserApprover = await _isUserApproverOfEquipmentDbQuery
            .ExecuteAsync(userId, cancellationToken);

        return !hasActiveAssignments && !isUserApprover;
    }
}

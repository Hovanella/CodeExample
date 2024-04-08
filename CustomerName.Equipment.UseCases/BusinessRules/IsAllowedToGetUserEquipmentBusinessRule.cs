using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsEquipmentUserFromDepartment;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.BusinessRules;

namespace CustomerName.Portal.Equipment.UseCases.BusinessRules;

internal interface IIsAllowedToGetUserEquipmentBusinessRule
{
    Task<bool> IsCorrespondsToBusiness(int? userId, CancellationToken cancellationToken);
}

public class IsAllowedToGetUserEquipmentBusinessRule : IIsAllowedToGetUserEquipmentBusinessRule,IBusinessRule<int?>
{
    private readonly IAuthenticatedUserContext _authenticatedUserContext;
    private readonly IIsEquipmentUserFromDepartmentDbQuery _isEquipmentUserFromDepartmentDbQuery;

    public IsAllowedToGetUserEquipmentBusinessRule(
        IAuthenticatedUserContext authenticatedUserContext,
        IIsEquipmentUserFromDepartmentDbQuery isEquipmentUserFromDepartmentDbQuery)
    {
        _authenticatedUserContext = authenticatedUserContext;
        _isEquipmentUserFromDepartmentDbQuery = isEquipmentUserFromDepartmentDbQuery;
    }

    public async Task<bool> IsCorrespondsToBusiness(int? userId, CancellationToken cancellationToken)
    {
        if (_authenticatedUserContext.UserId == userId)
        {
            return true;
        }

        return _authenticatedUserContext.Role switch
        {
            RoleType.CTO or RoleType.CEO or RoleType.SystemAdministrator => true,
            RoleType.HeadOfDepartment => await _isEquipmentUserFromDepartmentDbQuery.IsUserFromDepartment(userId!.Value,
                _authenticatedUserContext.DepartmentId,
                cancellationToken),
            _ => false
        };
    }
}

using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.BusinessRules;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UseCases.BusinessRules;

internal interface IIsAvailableEquipmentsWithoutAssignersBusinessRule
{
    Task<bool> IsCorrespondsToBusiness(IAuthenticatedUserContext? authenticatedUserContext, CancellationToken cancellationToken);
}

internal class IsAvailableEquipmentsWithoutAssignersBusinessRule : IBusinessRule<IAuthenticatedUserContext>,IIsAvailableEquipmentsWithoutAssignersBusinessRule
{
    private readonly IIdentityContract _identityContract;

    public IsAvailableEquipmentsWithoutAssignersBusinessRule(IIdentityContract identityContract)
    {
        _identityContract = identityContract;
    }

    public async Task<bool> IsCorrespondsToBusiness(IAuthenticatedUserContext? model,
        CancellationToken cancellationToken)
    {
        var isAvailableEquipmentsWithoutAssigners = model!.Role switch
        {
            RoleType.HeadOfDepartment => await _identityContract.IsAvailableEquipmentsWithoutAssignersAsync(
                model.DepartmentId!,
                cancellationToken),
            RoleType.SystemAdministrator => true,
            RoleType.Employee => false,
            RoleType.CEO => true,
            RoleType.CTO => true,
            _ => throw new ApplicationException("Unsupported RoleTypes")
        };
        return isAvailableEquipmentsWithoutAssigners;
    }
}

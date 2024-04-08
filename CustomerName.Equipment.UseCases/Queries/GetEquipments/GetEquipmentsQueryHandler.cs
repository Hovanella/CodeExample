using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.
    GetPageableEquipmentsWithFilterOptions;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentActiveHolder;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentApprovers;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Dto.FilterOptions;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipments;

internal class GetEquipmentsQueryHandler : IQueryHandler<GetEquipmentsQuery, PageableEquipmentDto>
{
    private readonly IGetPageableEquipmentsWithFilterOptionsDbQuery
        _getPageableEquipmentsWithFilterOptionsWithFilterOptionsDbQuery;
    private readonly IGetPossibleEquipmentApproversDbQuery _getPossibleEquipmentApproversDbQuery;
    private readonly IGetPossibleEquipmentActiveHoldersDbQuery _getPossibleEquipmentActiveHoldersDbQuery;
    private readonly IAuthenticatedUserContext _authenticatedUser;
    private readonly IIsAvailableEquipmentsWithoutAssignersBusinessRule _isAvailableEquipmentsWithoutAssigners;
    private readonly IIdentityContract _identityContract;

    public GetEquipmentsQueryHandler(
        IAuthenticatedUserContext authenticatedUser,
        IIsAvailableEquipmentsWithoutAssignersBusinessRule isAvailableEquipmentsWithoutAssigners,
        IIdentityContract identityContract,
        IGetPageableEquipmentsWithFilterOptionsDbQuery getPageableEquipmentsWithFilterOptionsWithFilterOptionsDbQuery,
        IGetPossibleEquipmentApproversDbQuery getPossibleEquipmentApproversDbQuery,
        IGetPossibleEquipmentActiveHoldersDbQuery getPossibleEquipmentActiveHoldersDbQuery)
    {
        _authenticatedUser = authenticatedUser;
        _isAvailableEquipmentsWithoutAssigners = isAvailableEquipmentsWithoutAssigners;
        _identityContract = identityContract;
        _getPageableEquipmentsWithFilterOptionsWithFilterOptionsDbQuery =
            getPageableEquipmentsWithFilterOptionsWithFilterOptionsDbQuery;
        _getPossibleEquipmentApproversDbQuery = getPossibleEquipmentApproversDbQuery;
        _getPossibleEquipmentActiveHoldersDbQuery = getPossibleEquipmentActiveHoldersDbQuery;
    }

    public async Task<PageableEquipmentDto> Handle(
        GetEquipmentsQuery query,
        CancellationToken cancellationToken)
    {
        var isAvailableEquipmentsWithoutAssigners = await _isAvailableEquipmentsWithoutAssigners
            .IsCorrespondsToBusiness(_authenticatedUser, cancellationToken);

        var pageableEquipmentsWithFilterOptions = await GetPageableEquipments(query, isAvailableEquipmentsWithoutAssigners, cancellationToken);

        var filterValues = await GetAvailableUsersFilterOptions(isAvailableEquipmentsWithoutAssigners, cancellationToken);

        return new PageableEquipmentDto()
        {
            FilterValues = new AvailableFilterOptionsDto
            {
                HasAvailable = pageableEquipmentsWithFilterOptions.HasAvailable,
                EquipmentLocations = pageableEquipmentsWithFilterOptions.EquipmentLocations,
                EquipmentTypeIds = pageableEquipmentsWithFilterOptions.EquipmentTypeIds,
                Approvers = filterValues.Approvers,
                Users = filterValues.Users,
            },
            Items = pageableEquipmentsWithFilterOptions.PageableEquipments.Items,
            Total = pageableEquipmentsWithFilterOptions.PageableEquipments.Total,
            Page = pageableEquipmentsWithFilterOptions.PageableEquipments.Page,
            PageSize = pageableEquipmentsWithFilterOptions.PageableEquipments.PageSize
        };
    }

    private async Task<GetPageableEquipmentsWithFilterOptionsDbQueryResponse> GetPageableEquipments(
        GetEquipmentsQuery query,
        bool isAvailableEquipmentsWithoutAssigners,
        CancellationToken cancellationToken)
    {
        var getPageableEquipmentsDbQueryRequest = new GetPageableEquipmentsDbQueryRequest(
            query.QueryOptions,
            _authenticatedUser.DepartmentId,
            isAvailableEquipmentsWithoutAssigners);

        var pageableEquipmentsWithFilterOptions = await _getPageableEquipmentsWithFilterOptionsWithFilterOptionsDbQuery
            .ExecuteAsync(getPageableEquipmentsDbQueryRequest, cancellationToken);

        if (_authenticatedUser.Role is RoleType.HeadOfDepartment)
        {
            pageableEquipmentsWithFilterOptions.PageableEquipments.Items.ToList().ForEach(e =>
            {
                e.PurchasePlace = null;
                e.PurchasePriceUsd = 0;
                e.PurchasePrice = 0;
            });
        }

        return pageableEquipmentsWithFilterOptions;
    }

    private async Task<AvailableUserFilterOptionsDto> GetAvailableUsersFilterOptions(
        bool isAvailableEquipmentsWithoutAssigners,
        CancellationToken cancellationToken)
    {
        var managersIds = await _identityContract.GetManagersIds(cancellationToken);
        var possibleEquipmentApprovers = await _getPossibleEquipmentApproversDbQuery.ExecuteAsync(
            managersIds,
            cancellationToken);

        var request = new GetPossibleEquipmentActiveHoldersDbQueryRequest(
            _authenticatedUser.DepartmentId,
            isAvailableEquipmentsWithoutAssigners);
        var possibleEquipmentActiveHolders = await _getPossibleEquipmentActiveHoldersDbQuery.ExecuteAsync(
            request,
            cancellationToken);

        var filterValues =
            new AvailableUserFilterOptionsDto(possibleEquipmentApprovers, possibleEquipmentActiveHolders);

        return filterValues;
    }
}

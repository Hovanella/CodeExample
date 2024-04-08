using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipmentsWithFilterOptions;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.OData.Extensions;
using CustomerName.Portal.Framework.Utils;
using CustomerName.Portal.Framework.Utils.Extensions;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.GetPageableEquipments;

internal class GetPageableEquipmentsWithFilterOptionsDbQuery : IGetPageableEquipmentsWithFilterOptionsDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;

    public GetPageableEquipmentsWithFilterOptionsDbQuery(
        IEquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public async Task<GetPageableEquipmentsWithFilterOptionsDbQueryResponse> ExecuteAsync(GetPageableEquipmentsDbQueryRequest request,
        CancellationToken cancellationToken)
    {
        var query = GetFilteredQuery(request);
        var queryOptionsWithoutPagination = request.QueryOptions.RebuildWithoutPagination();

        var result = await request.QueryOptions
            .ApplyTo(query)
            .Cast<OdpEquipment>()
            .ToListAsync(cancellationToken);

        result = result.DistinctBy(equipment => equipment.Id)
                       .ToList();

        var queryWithoutPagination = queryOptionsWithoutPagination
            .ApplyTo(query)
            .OfType<OdpEquipment>();

        var totalCount = await queryWithoutPagination.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return new GetPageableEquipmentsWithFilterOptionsDbQueryResponse(PageableListOfItems<OdpEquipment>
                .FromItems([])
                .WithPaging(page: 0, pageSize: 0)
                .WithTotal(0), false, [], []);
        }

        var pageableListOfItems = PageableListOfItems<OdpEquipment>
            .FromItems(result)
            .WithPaging((request.QueryOptions.Skip.Value / request.QueryOptions.Top.Value) + 1,
                request.QueryOptions.Top.Value)
            .WithTotal(totalCount);

        var hasAvailableEquipment = await queryWithoutPagination
            .AnyAsync(x => x.ActiveHolderId != null, cancellationToken);

        var equipmentTypeIds = await queryWithoutPagination
            .Select(equipment => equipment.EquipmentTypeId)
            .Distinct()
            .ToListAsync(cancellationToken);

        var equipmentLocations = await queryWithoutPagination
            .Select(equipment => equipment.Location)
            .Distinct()
            .ToListAsync(cancellationToken);

        return new GetPageableEquipmentsWithFilterOptionsDbQueryResponse(
            pageableListOfItems,
            hasAvailableEquipment,
            equipmentTypeIds,
            equipmentLocations.ConvertAll(EnumExtensions.ParseEnumValueByDescription<EquipmentLocationType>));
    }

    private IQueryable<OdpEquipment> GetFilteredQuery(GetPageableEquipmentsDbQueryRequest request)
    {
        var query = _equipmentDbContext
            .OdataEquipments.
            AsQueryable();

        if (!request.IsAvailableEquipmentsWithoutAssigners)
        {
            query = query.Where(x => x.ActiveHolderId != null);

            if (request.DepartmentId is not null)
            {
                query = query.Where(x => x.ActiveHolderDepartmentId == request.DepartmentId);
            }
        }

        return query;
    }
}

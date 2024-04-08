using Microsoft.EntityFrameworkCore;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipments;
using CustomerName.Portal.Equipment.Domain.OData;

namespace CustomerName.Portal.Equipment.DataAccess.Npgsql.DbQueries.GetPageableEquipments;

internal class GetEquipmentsDbQuery : IGetEquipmentsDbQuery
{
    private readonly IEquipmentDbContext _equipmentDbContext;

    public GetEquipmentsDbQuery(IEquipmentDbContext equipmentDbContext)
    {
        _equipmentDbContext = equipmentDbContext;
    }

    public Task<List<OdpEquipment>> ExecuteAsync(GetPageableEquipmentsDbQueryRequest request, CancellationToken cancellationToken)
    {
        var query = GetFilteredQuery(request);

        query = request.QueryOptions
                       .ApplyTo(query)
                       .Cast<OdpEquipment>();

        return query.GroupBy(equipment => equipment.Id)
                    .Select(group => group.First())
                    .ToListAsync(cancellationToken);
    }

    private IQueryable<OdpEquipment> GetFilteredQuery(GetPageableEquipmentsDbQueryRequest request)
    {
        var query = _equipmentDbContext
            .OdataEquipments.AsQueryable();

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

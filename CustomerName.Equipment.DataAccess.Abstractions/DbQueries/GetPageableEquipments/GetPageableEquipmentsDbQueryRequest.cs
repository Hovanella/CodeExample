using Microsoft.AspNetCore.OData.Query;
using CustomerName.Portal.Equipment.Domain.OData;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments;

internal record GetPageableEquipmentsDbQueryRequest(
    ODataQueryOptions<OdpEquipment> QueryOptions,
    string? DepartmentId,
    bool IsAvailableEquipmentsWithoutAssigners);

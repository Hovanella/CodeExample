using Microsoft.AspNetCore.OData.Query;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipments;

internal record GetEquipmentsQuery(ODataQueryOptions<OdpEquipment> QueryOptions)
    : IQuery<PageableEquipmentDto>, IEquipmentRequest;

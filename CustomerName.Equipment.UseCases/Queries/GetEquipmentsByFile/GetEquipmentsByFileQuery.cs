using Microsoft.AspNetCore.OData.Query;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByFile;

internal record GetEquipmentsByFileQuery(ODataQueryOptions<OdpEquipment> QueryOptions, DocumentType Format)
    : IQuery<byte[]>;

using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipments;

internal interface IGetEquipmentsDbQuery : IDbQuery<GetPageableEquipmentsDbQueryRequest, List<OdpEquipment>>;

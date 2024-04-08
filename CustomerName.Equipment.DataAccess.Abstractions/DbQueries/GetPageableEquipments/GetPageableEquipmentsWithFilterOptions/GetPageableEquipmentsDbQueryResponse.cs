using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Utils;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipmentsWithFilterOptions;

internal record GetPageableEquipmentsWithFilterOptionsDbQueryResponse(
    PageableListOfItems<OdpEquipment> PageableEquipments,
    bool HasAvailable,
    List<string> EquipmentTypeIds,
    List<EquipmentLocationType> EquipmentLocations);

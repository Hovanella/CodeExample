using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetAssignsByEquipmentId;

internal interface IGetAssignsByEquipmentIdDbQuery : IDbQuery<int, List<EquipmentAssign>>;

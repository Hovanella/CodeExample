using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentAssign;

internal interface ICreateEquipmentAssignDbQuery : IDbQuery<EquipmentAssign, CreateEquipmentAssignResponse>;

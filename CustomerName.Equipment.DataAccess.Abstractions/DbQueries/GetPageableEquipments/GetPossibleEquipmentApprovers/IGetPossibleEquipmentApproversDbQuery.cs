using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentApprovers;

internal interface
    IGetPossibleEquipmentApproversDbQuery : IDbQuery<List<int>, List<FilterUser>>;

using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetEquipmentUsersLookup;

public interface IGetEquipmentUsersLookupDbQuery : IDbQueryWithResponse<List<EquipmentUserLookup>>;


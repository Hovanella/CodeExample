using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

public interface IIsEquipmentExistByIdDbQuery : IDbQuery<int,bool>;

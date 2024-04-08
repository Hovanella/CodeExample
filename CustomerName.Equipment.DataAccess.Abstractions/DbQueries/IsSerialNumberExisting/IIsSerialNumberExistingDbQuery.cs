using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;

public interface IIsSerialNumberExistingDbQuery : IDbQuery<IsSerialNumberExistingDbRequest, bool>;

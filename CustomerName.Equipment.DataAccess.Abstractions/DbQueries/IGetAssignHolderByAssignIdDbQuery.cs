using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

public interface IGetAssignHolderByAssignIdDbQuery : IDbQuery<int,ActiveHolderDto>;

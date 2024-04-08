using CustomerName.Portal.Framework.DataAccess.Abstractions.DbQueries;

namespace CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;

internal interface IGetEquipmentApproverAndTypeByIdDbQuery : IDbQuery<int, Domain.Equipment?>
{
}

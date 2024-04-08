using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetEquipmentUsersLookup;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentUsersLookup;

public class GetEquipmentUsersLookupQueryHandler : IQueryHandler<GetEquipmentUsersLookupQuery,List<EquipmentUserLookup>>
{
    private readonly IGetEquipmentUsersLookupDbQuery _getEquipmentUsersLookupDbQuery;

    public GetEquipmentUsersLookupQueryHandler(IGetEquipmentUsersLookupDbQuery getEquipmentUsersLookupDbQuery)
    {
        _getEquipmentUsersLookupDbQuery = getEquipmentUsersLookupDbQuery;
    }

    public Task<List<EquipmentUserLookup>> Handle(GetEquipmentUsersLookupQuery request, CancellationToken cancellationToken)
    {
        return _getEquipmentUsersLookupDbQuery.ExecuteAsync(cancellationToken);
    }
}

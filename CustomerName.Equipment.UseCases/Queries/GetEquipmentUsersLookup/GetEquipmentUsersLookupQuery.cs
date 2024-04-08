using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentUsersLookup;

public record GetEquipmentUsersLookupQuery : IQuery<List<EquipmentUserLookup>>, IEquipmentRequest;

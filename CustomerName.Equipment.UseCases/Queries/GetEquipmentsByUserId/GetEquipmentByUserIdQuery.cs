using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByUserId;

internal record GetEquipmentByUserIdQuery(int UserId) : IQuery<List<UserEquipmentDto>>, IEquipmentRequest;

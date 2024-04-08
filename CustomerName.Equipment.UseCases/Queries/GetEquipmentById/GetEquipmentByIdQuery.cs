using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentById;

internal record GetEquipmentByIdQuery(int Id) : IQuery<EquipmentDto>, IEquipmentRequest;

using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentTypeClassificatorQuery;

public record GetEquipmentTypeClassificatorQuery : IQuery<List<EquipmentTypeClassificatorDto>>, IEquipmentRequest;

using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentTypeClassificatorQuery;

internal class GetEquipmentTypeClassificatorQueryHandler : IQueryHandler<GetEquipmentTypeClassificatorQuery,
    List<EquipmentTypeClassificatorDto>>
{
    private readonly IGetEquipmentTypesDbQuery _getEquipmentTypesDbQuery;

    public GetEquipmentTypeClassificatorQueryHandler(
        IGetEquipmentTypesDbQuery getEquipmentTypesDbQuery)
    {
        _getEquipmentTypesDbQuery = getEquipmentTypesDbQuery;
    }

    public async Task<List<EquipmentTypeClassificatorDto>> Handle(GetEquipmentTypeClassificatorQuery request,
        CancellationToken cancellationToken)
    {
        var equipmentTypes = await _getEquipmentTypesDbQuery.GetEquipmentTypes(cancellationToken);

        return equipmentTypes
            .Select(x => new EquipmentTypeClassificatorDto(x.Id, x.ShortName, x.FullName))
            .ToList();
    }
}

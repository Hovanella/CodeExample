using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportById;

internal class GetEquipmentReportByIdQueryHandler :
    IQueryHandler<GetEquipmentReportByIdQuery, EquipmentReportDto>
{
    private readonly IGetEquipmentReportByIdDbQuery _getEquipmentReportByIdDbQuery;
    private readonly IEquipmentReportToEquipmentReportDtoMapper _mapper;

    public GetEquipmentReportByIdQueryHandler(
        IGetEquipmentReportByIdDbQuery getEquipmentReportByIdDbQuery,
        IEquipmentReportToEquipmentReportDtoMapper mapper)
    {
        _getEquipmentReportByIdDbQuery = getEquipmentReportByIdDbQuery;
        _mapper = mapper;
    }

    public async Task<EquipmentReportDto> Handle(
        GetEquipmentReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        var equipmentReport = await _getEquipmentReportByIdDbQuery
                                    .GetEquipmentReportByIdAsync(
                                        request.Id,
                                        cancellationToken);

        if (equipmentReport is null)
        {
            throw new EntityNotFoundException(ExceptionConstants.EquipmentReportNotFound);
        }

        return _mapper.Map(equipmentReport);
    }
}

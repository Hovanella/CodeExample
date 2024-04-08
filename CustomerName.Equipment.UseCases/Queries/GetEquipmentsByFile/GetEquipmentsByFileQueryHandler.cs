using CustomerName.Portal.Csv.Contract.Equipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipments;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentExports;
using CustomerName.Portal.Excel.Contract.Equipments;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Queries;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByFile;

internal class GetEquipmentsByFileQueryHandler : IQueryHandler<GetEquipmentsByFileQuery, byte[]>
{
    private readonly IGetEquipmentsDbQuery _getEquipmentsDbQuery;
    private readonly IEquipmentToCsvExporter _csvExporter;
    private readonly IOdpEquipmentToEquipmentToCsvExportMapper _odpEquipmentToEquipmentToCsvExportMapper;
    private readonly IOdpEquipmentToEquipmentToExcelExportMapper _odpEquipmentToEquipmentToExcelExportMapper;
    private readonly IEquipmentToExcelExporter _excelExporter;
    private readonly IAuthenticatedUserContext _authenticatedUserContext;
    private readonly IIsAvailableEquipmentsWithoutAssignersBusinessRule _isAvailableEquipmentsWithoutAssigners;
    private readonly IIdentityContract _identityContract;

    public GetEquipmentsByFileQueryHandler(
        IEquipmentToCsvExporter csvExporter,
        IEquipmentToExcelExporter excelExporter,
        IAuthenticatedUserContext authenticatedUserContext,
        IIsAvailableEquipmentsWithoutAssignersBusinessRule isAvailableEquipmentsWithoutAssigners,
        IGetEquipmentsDbQuery getEquipmentsDbQuery,
        IOdpEquipmentToEquipmentToCsvExportMapper odpEquipmentToEquipmentToCsvExportMapper,
        IOdpEquipmentToEquipmentToExcelExportMapper odpEquipmentToEquipmentToExcelExportMapper,
        IIdentityContract identityContract)
    {
        _csvExporter = csvExporter;
        _excelExporter = excelExporter;
        _authenticatedUserContext = authenticatedUserContext;
        _isAvailableEquipmentsWithoutAssigners = isAvailableEquipmentsWithoutAssigners;
        _getEquipmentsDbQuery = getEquipmentsDbQuery;
        _odpEquipmentToEquipmentToCsvExportMapper = odpEquipmentToEquipmentToCsvExportMapper;
        _odpEquipmentToEquipmentToExcelExportMapper = odpEquipmentToEquipmentToExcelExportMapper;
        _identityContract = identityContract;
    }

    public async Task<byte[]> Handle(
        GetEquipmentsByFileQuery request,
        CancellationToken cancellationToken)
    {
        var isAvailableEquipmentsWithoutAssigners = await _isAvailableEquipmentsWithoutAssigners
            .IsCorrespondsToBusiness(_authenticatedUserContext, cancellationToken);

        var getPageableEquipmentsDbQueryRequest = new GetPageableEquipmentsDbQueryRequest(
            request.QueryOptions,
            _authenticatedUserContext.DepartmentId,
            isAvailableEquipmentsWithoutAssigners);

        var equipments = await _getEquipmentsDbQuery.ExecuteAsync(getPageableEquipmentsDbQueryRequest, cancellationToken);

        var activeHoldersDepartmentIds = equipments
            .Where(x => x.ActiveHolderDepartmentId is not null)
            .Select(x => x.ActiveHolderDepartmentId)
            .Distinct()
            .ToList();
        var departments = await _identityContract.GetDepartmentsByIds(activeHoldersDepartmentIds!, cancellationToken);

        var shouldPrintPurchaseDetails = _authenticatedUserContext.Role is RoleType.CEO or RoleType.CTO or RoleType.SystemAdministrator;

        return request.Format switch
        {
            DocumentType.Csv =>
                _csvExporter.Export(_odpEquipmentToEquipmentToCsvExportMapper.Map(equipments,departments),shouldPrintPurchaseDetails),

            DocumentType.Excel =>
                _excelExporter.Export(_odpEquipmentToEquipmentToExcelExportMapper.Map(equipments,departments),shouldPrintPurchaseDetails),

            _ => throw new ApplicationException("Unsupported file format")
        };
    }


}

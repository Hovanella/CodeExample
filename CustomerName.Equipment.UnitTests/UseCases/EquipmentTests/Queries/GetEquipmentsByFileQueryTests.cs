using Microsoft.AspNetCore.OData.Query;
using CustomerName.Portal.Csv.Contract.Equipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipments;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentExports;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByFile;
using CustomerName.Portal.Excel.Contract.Equipments;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.OData;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Queries;

public class GetEquipmentsByFileQueryTests
{
    private readonly Mock<IEquipmentToCsvExporter> _equipmentToCsvExporter = new();
    private readonly Mock<IEquipmentToExcelExporter> _equipmentToExcelExporter = new();
    private readonly Mock<IAuthenticatedUserContext> _authenticatedUserContextMock = new();
    private readonly Mock<IIsAvailableEquipmentsWithoutAssignersBusinessRule> _isAvailableEquipmentsWithoutAssignersMock = new();
    private readonly Mock<IGetEquipmentsDbQuery> _getEquipmentsDbQuery = new();
    private readonly Mock<IOdpEquipmentToEquipmentToCsvExportMapper> _odpEquipmentToEquipmentToCsvExportMapper = new();
    private readonly Mock<IOdpEquipmentToEquipmentToExcelExportMapper> _odpEquipmentToEquipmentToExcelExportMapper = new();
    private readonly Mock<IIdentityContract> _identityContract = new();

    private readonly CancellationToken _cancellationToken = new();
    private readonly GetEquipmentsByFileQueryHandler _handler;

    public GetEquipmentsByFileQueryTests()
    {
        _handler = new GetEquipmentsByFileQueryHandler(
            _equipmentToCsvExporter.Object,
            _equipmentToExcelExporter.Object,
            _authenticatedUserContextMock.Object,
            _isAvailableEquipmentsWithoutAssignersMock.Object,
            _getEquipmentsDbQuery.Object,
            _odpEquipmentToEquipmentToCsvExportMapper.Object,
            _odpEquipmentToEquipmentToExcelExportMapper.Object,
            _identityContract.Object);
    }

    [Fact]
    public async Task GetEquipmentsByFile_WhenTryToGetCsvFile_ReturnStreamWithFileOfCsvFormat()
    {
        // Arrange
        var equipment = new OdpEquipment()
        {
            Id = 1,
            Name = "Name",
            Characteristics = "Characteristics",
            PurchasePlace = "PurchasePlace",
            SerialNumber = "SerialNumber",
            Location = LocationType.Minsk.ToString(),
            PurchaseCurrency = "Minsk",
            Comment = "comment",
            ActiveHolderId = 99,
            ActiveHolderIssueDate = new DateTime(2023, 04, 10, 0, 0, 0, DateTimeKind.Utc),
            ActiveHolderReturnDate =  new (2023, 05, 10, 0, 0, 0, DateTimeKind.Utc),
            ActiveHolderFullName = "Last First",
            ActiveHolderDepartmentId = "Net",
            ApproverFullName = "Head of Department Test",
        };

        var streamWithCsv = Array.Empty<byte>();
        var equipmentToCsvExport = new EquipmentToCsvExport
        {
            Id = 1,
            Name = "EquipmentName",
            TypeFullName = "Keyboard",
            SerialNumber = "123131231243",
            PurchasePlace = "Minsk",
            Characteristics = "TestCharacteristics",
            Location = LocationType.Minsk.ToString(),
        };

        var equipments = new List<OdpEquipment>() { equipment };

        var queryParameters = new Dictionary<string, string?>
        {
            [nameof(ODataRawQueryOptions.Top)] = "50", [nameof(ODataRawQueryOptions.Skip)] = "0",
        };

        var queryOptions = ODataQueryOptionsMock.BuildMock<OdpEquipment>(queryParameters);

        _getEquipmentsDbQuery
            .Setup(x => x.ExecuteAsync(new GetPageableEquipmentsDbQueryRequest(queryOptions, "Net", true), _cancellationToken))
            .ReturnsAsync(equipments);

        _equipmentToCsvExporter
            .Setup(x => x.Export(It.IsAny<IEnumerable<EquipmentToCsvExport>>(),It.IsAny<bool>()))
            .Returns(streamWithCsv);

        _authenticatedUserContextMock
            .Setup(x => x.DepartmentId)
            .Returns("Net");

        _isAvailableEquipmentsWithoutAssignersMock
            .Setup(a => a.IsCorrespondsToBusiness(_authenticatedUserContextMock.Object, _cancellationToken))
            .ReturnsAsync(true);

        var request = new GetEquipmentsByFileQuery(queryOptions, DocumentType.Csv);

        // Act
        var result = await _handler.Handle(request, _cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(streamWithCsv);
    }

    [Fact]
    public async Task GetEquipmentsByFile_WhenTryToGetExcelFile_ReturnStreamWithFileOfExcelFormat()
    {
        // Arrange
        var equipment = new OdpEquipment()
        {
            Id = 1,
            Name = "Name",
            Characteristics = "Characteristics",
            PurchasePlace = "PurchasePlace",
            SerialNumber = "SerialNumber",
            Location = LocationType.Minsk.ToString(),
            PurchaseCurrency = "Minsk",
            Comment = "comment",
            ActiveHolderId = 99,
            ActiveHolderIssueDate = new DateTime(2023, 04, 10, 0, 0, 0, DateTimeKind.Utc),
            ActiveHolderReturnDate =  new (2023, 05, 10, 0, 0, 0, DateTimeKind.Utc),
            ActiveHolderFullName = "Last First",
            ActiveHolderDepartmentId = "Net",
            ApproverFullName = "Head of Department Test",
        };

        var streamWithExcelFile = Array.Empty<byte>();
        var equipmentToExcelExport = new EquipmentToExcelExport
        {
            Id = 4,
            Name = "EquipmentName",
            Type = "Keyboard",
            SerialNumber = "123131231243",
            PurchasePlace = "Minsk",
            Characteristics = "TestCharacteristics",
            Location = LocationType.Minsk.ToString(),
        };

        var equipments = new List<OdpEquipment>() { equipment };

        var queryParameters = new Dictionary<string, string?>
        {
            [nameof(ODataRawQueryOptions.Top)] = "50", [nameof(ODataRawQueryOptions.Skip)] = "0",
        };

        var queryOptions = ODataQueryOptionsMock.BuildMock<OdpEquipment>(queryParameters);

        _getEquipmentsDbQuery
            .Setup(x => x.ExecuteAsync(new GetPageableEquipmentsDbQueryRequest(queryOptions, "Net", true), _cancellationToken))
            .ReturnsAsync(equipments);

        _equipmentToExcelExporter
            .Setup(x => x.Export(It.IsAny<IEnumerable<EquipmentToExcelExport>>(),true))
            .Returns(streamWithExcelFile);

        _authenticatedUserContextMock
            .Setup(x => x.Role)
            .Returns(RoleType.SystemAdministrator);

        _authenticatedUserContextMock
            .Setup(x => x.DepartmentId)
            .Returns("Net");

        _isAvailableEquipmentsWithoutAssignersMock
            .Setup(a => a.IsCorrespondsToBusiness(_authenticatedUserContextMock.Object, _cancellationToken))
            .ReturnsAsync(true);

        var request = new GetEquipmentsByFileQuery(queryOptions, DocumentType.Excel);

        // Act
        var result = await _handler.Handle(request, _cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(streamWithExcelFile);
    }
}

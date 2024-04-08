using System.Text;
using Microsoft.AspNetCore.Http;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentReport;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipmentReport;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentReportTests.Commands;

public class CreateEquipmentReportCommandTests
{
    private readonly Mock<ICreateEquipmentReportDbQuery> _createEquipmentReportDbQueryMock = new();
    private readonly Mock<IGetEquipmentReportBySerialNumberAndDataHashDbQuery> _getEquipmentReportDbQueryMock = new();
    private readonly Mock<IUpdateEquipmentReportRelevancePeriodDbQuery> _updateEquipmentReportPeriodDbQueryMock = new();
    private readonly Mock<IIsEquipmentExistingBySerialNumberDbQuery> _isEquipmentExistingDbQueryMock = new();

    private readonly Mock<IHashService> _hashServiceMock = new();

    private readonly Mock<IEquipmentReportToEquipmentReportDtoMapper> _mapperMock = new();

    private readonly CancellationToken _cancellationToken;

    private readonly CreateEquipmentReportCommandHandler _handler;

    public CreateEquipmentReportCommandTests()
    {
        _cancellationToken = CancellationToken.None;
        _handler = new CreateEquipmentReportCommandHandler(
            _createEquipmentReportDbQueryMock.Object,
            _getEquipmentReportDbQueryMock.Object,
            _updateEquipmentReportPeriodDbQueryMock.Object,
            _isEquipmentExistingDbQueryMock.Object,
            _mapperMock.Object,
            _hashServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenHardwareSerialNumberNotFound_ShouldThrowInvalidDataAppException()
    {
        //Arrange
        var jsonData = "{\"data\": \"info\", \"CreationDate\": \"2024-01-29T06:51:24Z\"}";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));

        var formFileMock = new Mock<IFormFile>();

        formFileMock.Setup(formFile => formFile.OpenReadStream())
                    .Returns(stream);

        var request = new CreateEquipmentReportCommand(formFileMock.Object);

        //Act
        var result = () => _handler.Handle(request, _cancellationToken);

        //Assert
        await result.Should().ThrowAsync<InvalidDataAppException>();

        formFileMock.Verify();
    }

    [Fact]
    public async Task Handle_WhenEquipmentIsNotExisting_ShouldThrowInvalidDataAppException()
    {
        //Arrange
        var serialNumber = "SKVKAFA";
        var jsonData = "{\"data\": \"info\", \"CreationDate\": \"2024-01-29T06:51:24Z\"," +
                       "\"HardwareSerialNumber\": \"SKVKAFA\"}";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));

        var formFileMock = new Mock<IFormFile>();

        formFileMock.Setup(formFile => formFile.OpenReadStream())
                    .Returns(stream);

        var request = new CreateEquipmentReportCommand(formFileMock.Object);

        _isEquipmentExistingDbQueryMock.Setup(isEquipmentExistingDbQuery =>
            isEquipmentExistingDbQuery.IsEquipmentExistingBySerialNumberAsync(
                serialNumber.ToLower(),
                _cancellationToken))
            .ReturnsAsync(false);

        //Act
        var result = () => _handler.Handle(request, _cancellationToken);

        //Assert
        await result.Should().ThrowAsync<InvalidDataAppException>();

        formFileMock.Verify();
    }

    [Fact]
    public async Task Handle_WhenCreationDateNotFound_ShouldThrowInvalidDataAppException()
    {
        //Arrange
        var jsonData = "{\"data\": \"info\", \"HardwareSerialNumber\": \"SKVKAFA\"}";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));

        var formFileMock = new Mock<IFormFile>();

        formFileMock.Setup(formFile => formFile.OpenReadStream())
                    .Returns(stream);

        var request = new CreateEquipmentReportCommand(formFileMock.Object);

        //Act
        var result = () => _handler.Handle(request, _cancellationToken);

        //Assert
        await result.Should().ThrowAsync<InvalidDataAppException>();

        formFileMock.Verify();
    }

    [Fact]
    public async Task Handle_WhenEquipmentReportNotFound_ShouldReturnEquipmentReportDto()
    {
        //Arrange
        var serialNumber = "SKVKAFA";
        var creationDate = DateTime.Parse("2024-01-29T06:51:24Z").ToUniversalTime();

        var jsonData = "{\"data\": \"info\", \"CreationDate\": \"2024-01-29T06:51:24Z\"," +
                       "\"HardwareSerialNumber\": \"SKVKAFA\"}";

        var resultJsonData = "{\"data\":\"info\"}";

        var jsonDataHash = "somehash";

        _isEquipmentExistingDbQueryMock.Setup(isEquipmentExistingDbQuery =>
            isEquipmentExistingDbQuery.IsEquipmentExistingBySerialNumberAsync(
                serialNumber,
                _cancellationToken))
            .ReturnsAsync(true);

        _hashServiceMock.Setup(hashService =>
            hashService.GetSha256Hash(resultJsonData))
            .Returns(jsonDataHash);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));

        var formFileMock = new Mock<IFormFile>();

        formFileMock.Setup(formFile => formFile.OpenReadStream())
                    .Returns(stream);

        var request = new CreateEquipmentReportCommand(formFileMock.Object);

        _getEquipmentReportDbQueryMock.Setup(getEquipmentReportDbQuery =>
            getEquipmentReportDbQuery.GetEquipmentReportBySerialNumberAndDataHashAsync(
                serialNumber,
                jsonDataHash,
                _cancellationToken))
            .ReturnsAsync((EquipmentReport?)null);

        var dbQueryRequest = new CreateEquipmentReportDbQueryRequest(
            serialNumber.ToLower(),
            resultJsonData,
            jsonDataHash,
            creationDate);

        var equipmentReport = new EquipmentReport
        {
            Id = 5,
            Data = resultJsonData,
            DataHash = jsonDataHash,
            SerialNumber = serialNumber.ToLower(),
            AssembledAtUtc = creationDate,
            CreatedAtUtc = DateTime.UtcNow
        };

        var equipmentReportDto = new EquipmentReportDto
        {
            Id = 5,
            Data = resultJsonData,
            SerialNumber = serialNumber.ToLower(),
            AssembledAtUtc = creationDate,
            CreatedAtUtc = DateTime.UtcNow
        };

        _createEquipmentReportDbQueryMock.Setup(createEquipmentReportDbQuery =>
            createEquipmentReportDbQuery.CreateEquipmentReportAsync(
                It.Is<CreateEquipmentReportDbQueryRequest>(request =>
                    request.Data == dbQueryRequest.Data &&
                    request.DataHash == dbQueryRequest.DataHash &&
                    request.SerialNumber == dbQueryRequest.SerialNumber &&
                    request.AssembledAtUtc == dbQueryRequest.AssembledAtUtc),
                _cancellationToken))
            .ReturnsAsync(equipmentReport);

        _mapperMock.Setup(mapper =>
            mapper.Map(equipmentReport))
            .Returns(equipmentReportDto);

        //Act
        var result = await _handler.Handle(request, _cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(equipmentReportDto);

        formFileMock.Verify();
        _hashServiceMock.Verify();
        _getEquipmentReportDbQueryMock.Verify();
        _createEquipmentReportDbQueryMock.Verify();
        _mapperMock.Verify();
    }

    [Fact]
    public async Task Handle_WhenEquipmentReportExists_ShouldReturnEquipmentReportDto()
    {
        //Arrange
        var serialNumber = "SKVKAFA";
        var creationDate = DateTime.Parse("2024-01-29T06:51:24Z").ToUniversalTime();

        var jsonData = "{\"data\": \"info\", \"CreationDate\": \"2024-01-29T06:51:24Z\"," +
                       "\"HardwareSerialNumber\": \"SKVKAFA\"}";

        var resultJsonData = "{\"data\":\"info\"}";

        var jsonDataHash = "somehash";

        _isEquipmentExistingDbQueryMock.Setup(isEquipmentExistingDbQuery =>
            isEquipmentExistingDbQuery.IsEquipmentExistingBySerialNumberAsync(
                serialNumber,
                _cancellationToken))
            .ReturnsAsync(true);

        _hashServiceMock.Setup(hashService =>
            hashService.GetSha256Hash(resultJsonData))
            .Returns(jsonDataHash);

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonData));

        var formFileMock = new Mock<IFormFile>();

        formFileMock.Setup(formFile => formFile.OpenReadStream())
                    .Returns(stream);

        var request = new CreateEquipmentReportCommand(formFileMock.Object);

        var equipmentReport = new EquipmentReport
        {
            Id = 6,
            Data = resultJsonData,
            DataHash = jsonDataHash,
            SerialNumber = serialNumber.ToLower(),
            AssembledAtUtc = creationDate,
            CreatedAtUtc = DateTime.UtcNow
        };

        var equipmentReportDto = new EquipmentReportDto
        {
            Id = 6,
            Data = resultJsonData,
            SerialNumber = serialNumber.ToLower(),
            AssembledAtUtc = creationDate,
            CreatedAtUtc = DateTime.UtcNow
        };

        _getEquipmentReportDbQueryMock.Setup(getEquipmentReportDbQuery =>
            getEquipmentReportDbQuery.GetEquipmentReportBySerialNumberAndDataHashAsync(
                serialNumber.ToLower(),
                jsonDataHash,
                _cancellationToken))
            .ReturnsAsync(equipmentReport);

        _updateEquipmentReportPeriodDbQueryMock.Setup(updateEquipmentReportPeriodDbQuery =>
            updateEquipmentReportPeriodDbQuery.UpdateEquipmentReportRelevancePeriodToUtcDateAsync(
                equipmentReport.SerialNumber,
                equipmentReport.Id,
                _cancellationToken));

        _mapperMock.Setup(mapper =>
            mapper.Map(equipmentReport))
            .Returns(equipmentReportDto);

        //Act
        var result = await _handler.Handle(request, _cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(equipmentReportDto);

        formFileMock.Verify();
        _hashServiceMock.Verify();
        _getEquipmentReportDbQueryMock.Verify();
        _updateEquipmentReportPeriodDbQueryMock.Verify();
        _mapperMock.Verify();
    }
}

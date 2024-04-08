using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportById;
using CustomerName.Portal.Framework.Exceptions;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentReportTests.Queries;

public class GetEquipmentReportByIdQueryTests
{
    private readonly Mock<IGetEquipmentReportByIdDbQuery> _getEquipmentReportByIdDbQueryMock = new();
    private readonly Mock<IEquipmentReportToEquipmentReportDtoMapper> _mapperMock = new();

    private readonly CancellationToken _cancellationToken;
    private readonly GetEquipmentReportByIdQueryHandler _handler;

    public GetEquipmentReportByIdQueryTests()
    {
        _cancellationToken = CancellationToken.None;
        _handler = new GetEquipmentReportByIdQueryHandler(
            _getEquipmentReportByIdDbQueryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEquipmentReportDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        //Arrange
        var equipmentReportId = 1;
        var request = new GetEquipmentReportByIdQuery(equipmentReportId);

        _getEquipmentReportByIdDbQueryMock.Setup(getEquipmentReportById =>
            getEquipmentReportById.GetEquipmentReportByIdAsync(equipmentReportId, _cancellationToken))
            .ReturnsAsync((EquipmentReport?)null);

        //Act
        var result = () => _handler.Handle(request, _cancellationToken);

        //Assert
        await result.Should().ThrowAsync<EntityNotFoundException>();

        _getEquipmentReportByIdDbQueryMock.Verify();
    }

    [Fact]
    public async Task Handle_WhenEquipmentReportExists_ShouldReturnEquipmentReportDto()
    {
        //Arrange
        var equipmentReportId = 2;
        var request = new GetEquipmentReportByIdQuery(equipmentReportId);

        var expected = new EquipmentReportDto
        {
            Id = 2,
            SerialNumber = "GKSLAVWQ",
            Data = "{\"data\": \"info\"}",
            AssembledAtUtc = DateTime.UtcNow,
            CreatedAtUtc = DateTime.UtcNow
        };

        var equipmentReport = new EquipmentReport
        {
            Id = expected.Id,
            SerialNumber = expected.SerialNumber,
            Data = expected.Data,
            DataHash = string.Empty,
            AssembledAtUtc = expected.AssembledAtUtc,
            CreatedAtUtc = expected.CreatedAtUtc
        };

        _getEquipmentReportByIdDbQueryMock.Setup(getEquipmentReportById =>
            getEquipmentReportById.GetEquipmentReportByIdAsync(equipmentReportId, _cancellationToken))
            .ReturnsAsync(equipmentReport);

        _mapperMock.Setup(mapper => mapper.Map(equipmentReport))
            .Returns(expected);

        //Act
        var result = await _handler.Handle(request, _cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(expected);

        _getEquipmentReportByIdDbQueryMock.Verify();
        _mapperMock.Verify();
    }
}

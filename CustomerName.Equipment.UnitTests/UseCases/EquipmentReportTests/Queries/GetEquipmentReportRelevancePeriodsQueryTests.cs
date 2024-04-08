using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportRelevancePeriods;
using CustomerName.Portal.Framework.Exceptions;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentReportTests.Queries;

public class GetEquipmentReportRelevancePeriodsQueryTests
{
    private readonly Mock<IGetEquipmentReportRelevancePeriodsDbQuery> _getEquipmentReportRelevancePeriodsMock = new();
    private readonly Mock<IReportRelevancePeriodToReportRelevancePeriodDtoMapper> _mapperMock = new();

    private readonly CancellationToken _cancellationToken;
    private readonly GetEquipmentReportRelevancePeriodsQueryHandler _handler;

    public GetEquipmentReportRelevancePeriodsQueryTests()
    {
        _cancellationToken = CancellationToken.None;
        _handler = new GetEquipmentReportRelevancePeriodsQueryHandler(
            _getEquipmentReportRelevancePeriodsMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEquipmentReportRelevancePeriodsNotFound_ShouldThrowEntityNotFoundException()
    {
        //Arrange
        var serialNumber = "ISKASFJQ";
        var request = new GetEquipmentReportRelevancePeriodsQuery(serialNumber);

        _getEquipmentReportRelevancePeriodsMock.Setup(getEquipmentReportById =>
            getEquipmentReportById.GetEquipmentReportRelevancePeriodsBySerialNumberAsync(
                serialNumber,
                _cancellationToken))
            .ReturnsAsync((List<EquipmentReportRelevancePeriod>?)null);

        //Act
        var result = () => _handler.Handle(request, _cancellationToken);

        //Assert
        await result.Should().ThrowAsync<EntityNotFoundException>();

        _getEquipmentReportRelevancePeriodsMock.Verify();
    }

    [Fact]
    public async Task Handle_WhenEquipmentReportRelevancePeriodsCountIsZero_ShouldThrowEntityNotFoundException()
    {
        //Arrange
        var serialNumber = "ISKASFJQ";
        var request = new GetEquipmentReportRelevancePeriodsQuery(serialNumber);

        _getEquipmentReportRelevancePeriodsMock.Setup(getEquipmentReportById =>
            getEquipmentReportById.GetEquipmentReportRelevancePeriodsBySerialNumberAsync(
                serialNumber,
                _cancellationToken))
            .ReturnsAsync(new List<EquipmentReportRelevancePeriod>());

        //Act
        var result = () => _handler.Handle(request, _cancellationToken);

        //Assert
        await result.Should().ThrowAsync<EntityNotFoundException>();

        _getEquipmentReportRelevancePeriodsMock.Verify();
    }

    [Fact]
    public async Task Handle_WhenItIsFoundAndCountGreaterThanZero_ShouldReturnEquipmentReportRelevancePeriodDtoList()
    {
        //Arrange
        var serialNumber = "ISKASFJQ";
        var request = new GetEquipmentReportRelevancePeriodsQuery(serialNumber);

        var equipmentReportRelevancePeriods = new List<EquipmentReportRelevancePeriod>
        {
            new EquipmentReportRelevancePeriod
            {
                FromUtc = new DateTime(2023, 9, 15).ToUniversalTime(),
                ToUtc = new DateTime(2023, 9, 16).ToUniversalTime(),
                EquipmentReportId = 3
            },
            new EquipmentReportRelevancePeriod
            {
                FromUtc = new DateTime(2023, 9, 17).ToUniversalTime(),
                ToUtc = new DateTime(2023, 9, 18).ToUniversalTime(),
                EquipmentReportId = 4
            }
        };

        var equipmentReportRelevancePeriodDtos = new List<EquipmentReportRelevancePeriodDto>
        {
            new EquipmentReportRelevancePeriodDto
            {
                FromUtc = new DateTime(2023, 9, 15).ToUniversalTime(),
                ToUtc = new DateTime(2023, 9, 16).ToUniversalTime(),
                EquipmentReportId = 3
            },
            new EquipmentReportRelevancePeriodDto
            {
                FromUtc = new DateTime(2023, 9, 17).ToUniversalTime(),
                ToUtc = new DateTime(2023, 9, 18).ToUniversalTime(),
                EquipmentReportId = 4
            }
        };

        _getEquipmentReportRelevancePeriodsMock.Setup(getEquipmentReportById =>
            getEquipmentReportById.GetEquipmentReportRelevancePeriodsBySerialNumberAsync(
                serialNumber,
                _cancellationToken))
            .ReturnsAsync(equipmentReportRelevancePeriods);

        _mapperMock.Setup(mapper =>
            mapper.Map(equipmentReportRelevancePeriods))
            .Returns(equipmentReportRelevancePeriodDtos);

        //Act
        var result = await _handler.Handle(request, _cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(equipmentReportRelevancePeriodDtos);

        _getEquipmentReportRelevancePeriodsMock.Verify();
        _mapperMock.Verify();
    }
}

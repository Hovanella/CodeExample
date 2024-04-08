using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentHistory;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Queries;

public class GetEquipmentHistoryTests
{
    private readonly Mock<IEquipmentAssignProvider> _equipmentAssignProviderMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly CancellationToken _cancellationToken = new();
    private readonly GetEquipmentHistoryQueryHandler _handler;

    public GetEquipmentHistoryTests()
    {
        _handler = new GetEquipmentHistoryQueryHandler(
            _equipmentAssignProviderMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetEquipmentHistory_WhenEquipmentIsNotAssignedToUsers_ReturnEmptyEquipmentHoldersList()
    {
        // Arrange
        _equipmentAssignProviderMock
            .Setup(x => x.GetByEquipmentIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync([]);
        _mapperMock
            .Setup(x => x.Map<List<EquipmentHolderDto>>(It.IsAny<List<EquipmentAssign>>()))
            .Returns([]);

        var request = new GetEquipmentHistoryQuery(1);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEquipmentHistory_WhenEquipmentIsAssignedToUsers_ReturnNotEmptyEquipmentHoldersList()
    {
        // Arrange
        var equipmentAssigns = new List<EquipmentAssign>
        {
            new()
            {
                AssignedToUserId = 99
            }
        };

        var equipmentHolders = new List<EquipmentHolderDto>
        {
            new()
            {
                Id = 99,
                FirstName = "John",
                LastName = "Doe"
            }
        };

        _equipmentAssignProviderMock
            .Setup(x => x.GetByEquipmentIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync(equipmentAssigns);
        _mapperMock
            .Setup(x => x.Map<List<EquipmentHolderDto>>(equipmentAssigns))
            .Returns(equipmentHolders);

        var request = new GetEquipmentHistoryQuery(1);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainSingle();
    }
}

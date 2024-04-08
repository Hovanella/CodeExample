using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByUserId;
using CustomerName.Portal.Framework.Exceptions;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Queries;

public class GetEquipmentByUserIdQueryTests
{
    private readonly Mock<IEquipmentAssignProvider> _assignEquipmentProviderMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IEquipmentUserProvider> _equipmentUserProviderMock = new();
    private readonly Mock<IIsAllowedToGetUserEquipmentBusinessRule> _isAllowedToGetUserEquipmentBusinessRuleMock =
        new();

    private readonly CancellationToken _cancellationToken;
    private readonly GetEquipmentByUserIdQueryHandler _handler;

    public GetEquipmentByUserIdQueryTests()
    {
        _cancellationToken = CancellationToken.None;
        _handler = new GetEquipmentByUserIdQueryHandler(_assignEquipmentProviderMock.Object,
            _mapperMock.Object,
            _equipmentUserProviderMock.Object,
            _isAllowedToGetUserEquipmentBusinessRuleMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExists_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var request = new GetEquipmentByUserIdQuery(1234);

        _equipmentUserProviderMock.Setup(x => x.GetUserByIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync((EquipmentUser?)null);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotHavePermission_ShouldThrowActionNotAllowedException()
    {
        // Arrange
        var request = new GetEquipmentByUserIdQuery(1);
        var user = new EquipmentUser
        {
            UserId = 1,
            DepartmentId = "Net",
            Email = "CustomerName@CustomerName-software.com",
            FirstName = "Test",
            LastName = "Test"
        };

        _equipmentUserProviderMock.Setup(x => x.GetUserByIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync(user);
        _isAllowedToGetUserEquipmentBusinessRuleMock
            .Setup(x => x.IsCorrespondsToBusiness(It.IsAny<int?>(),_cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<ActionNotAllowedException>();
    }

    [Fact]
    public async Task Handle_WhenEquipmentExists_ShouldReturnMappedEquipment()
    {
        // Arrange
        var request = new GetEquipmentByUserIdQuery(1234);
        var user = new EquipmentUser
        {
            UserId = 1,
            DepartmentId = "Net",
            Email = "CustomerName@CustomerName-software.com",
            FirstName = "Test",
            LastName = "Test"
        };

        var equipments = new List<EquipmentAssign>
        {
            new EquipmentAssign
            {
                Id = 1,
                AssignedToUserId = 1,
                EquipmentId = 1,
                User = user,
                IssueDate = new DateTime(2022, 7, 12, 0, 0, 0, DateTimeKind.Utc),
                ReturnDate = new DateTime(2024, 7, 12, 0, 0, 0, DateTimeKind.Utc),
                Equipment = new Domain.Equipment
                {
                    Name = "Test",
                    TypeId = "DesktopComputer",
                    SerialNumber = "12345678",
                    Characteristics = "Characteristics",
                    PurchasePlace = "PurchasePlace"
                }
            }
        };

        var equipmentsDto = new List<UserEquipmentDto>
        {
            new UserEquipmentDto
            {
                EquipmentName = "Test",
                EquipmentTypeId = "DesktopComputer",
                IssueDate = new DateTime(2022, 7, 12, 0, 0, 0, DateTimeKind.Utc),
                ReturnDate = new DateTime(2024, 7, 12, 0, 0, 0, DateTimeKind.Utc),
                SerialNumber = "12345678"
            }
        };

        var expectedEquipmentDto = new List<UserEquipmentDto>
        {
            new UserEquipmentDto
            {
                EquipmentName = "Test",
                EquipmentTypeId = "DesktopComputer",
                IssueDate = new DateTime(2022, 7, 12, 0, 0, 0, DateTimeKind.Utc),
                ReturnDate = new DateTime(2024, 7, 12, 0, 0, 0, DateTimeKind.Utc),
                SerialNumber = "12345678"
            }
        };

        _equipmentUserProviderMock.Setup(x => x.GetUserByIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync(user);
        _assignEquipmentProviderMock.Setup(x => x.GetEquipmentsByUserIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync(equipments);
        _isAllowedToGetUserEquipmentBusinessRuleMock
            .Setup(x => x.IsCorrespondsToBusiness(It.IsAny<int?>(), _cancellationToken))
            .ReturnsAsync(true);
        _mapperMock.Setup(x => x.Map<List<UserEquipmentDto>>(equipments))
            .Returns(equipmentsDto);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(expectedEquipmentDto);
    }

    [Fact]
    public async Task Handle_WhenEquipmentDoesNotExist_ShouldReturnEmptyList()
    {
        // Arrange
        var request = new GetEquipmentByUserIdQuery(1234);
        var user = new EquipmentUser
        {
            UserId = 1,
            DepartmentId = "Net",
            Email = "CustomerName@CustomerName-software.com",
            FirstName = "Test",
            LastName = "Test"
        };

        var equipments = new List<EquipmentAssign>();

        var equipmentsDto = new List<UserEquipmentDto>();

        _equipmentUserProviderMock.Setup(x => x.GetUserByIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync(user);
        _assignEquipmentProviderMock.Setup(x => x.GetEquipmentsByUserIdAsync(
                It.IsAny<int>(),
                _cancellationToken))
            .ReturnsAsync(equipments);
        _isAllowedToGetUserEquipmentBusinessRuleMock
            .Setup(x => x.IsCorrespondsToBusiness(It.IsAny<int?>(), _cancellationToken))
            .ReturnsAsync(true);
        _mapperMock.Setup(x => x.Map<List<UserEquipmentDto>>(equipments))
            .Returns(equipmentsDto);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().BeEmpty();
    }
}
using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Commands.DeleteEquipmentHistoryRecord;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Exceptions;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Commands;

public class DeleteEquipmentHistoryCommandTests
{
    private readonly Mock<IEquipmentProvider> _equipmentProviderMock = new();
    private readonly Mock<IEquipmentAssignRepository> _equipmentAssignRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly CancellationToken _cancellationToken;
    private readonly DeleteEquipmentHistoryCommandHandler _handler;

    public DeleteEquipmentHistoryCommandTests()
    {
        _cancellationToken = CancellationToken.None;

        _handler = new DeleteEquipmentHistoryCommandHandler(
            _equipmentProviderMock.Object,
            _equipmentAssignRepositoryMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEquipmentDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var equipmentId = 1;

        var request = new DeleteEquipmentHistoryCommand(equipmentId);

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync((Domain.Equipment?)null);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<EntityNotFoundException>();
        _equipmentAssignRepositoryMock.Verify(
            x => x.DeleteEquipmentAssignmentAsync(
                equipmentId,
                _cancellationToken),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenEquipmentDoesNotContainAssigns_InvalidDataAppException()
    {
        // Arrange
        var equipmentId = 2;

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            Name = "TestName",
            TypeId = "Laptop",
            SerialNumber = "00000000001",
            PurchasePrice = 1000,
            PurchasePriceUsd = 300,
            PurchaseDate = DateTime.Today.Date,
            PurchasePlace = "Minsk",
            GuaranteeDate = DateTime.Today.AddYears(1).Date,
            Characteristics = "Test characteristics",
            Location = EquipmentLocationType.Belarus,
            Comment = "Test comment",
            Assigns = []
        };

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(equipment);

        var request = new DeleteEquipmentHistoryCommand(equipmentId);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<InvalidDataAppException>();
        _equipmentAssignRepositoryMock.Verify(
            x => x.DeleteEquipmentAssignmentAsync(
                equipmentId,
                _cancellationToken),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenEquipmentDoesNotContainActiveAssigns_InvalidDataAppException()
    {
        // Arrange
        var equipmentId = 3;

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            Name = "TestName",
            TypeId = "Laptop",
            SerialNumber = "00000000001",
            PurchasePrice = 1000,
            PurchasePriceUsd = 300,
            PurchaseDate = new DateTime(2023, 7, 1, 0, 0, 0, DateTimeKind.Utc),
            PurchasePlace = "Minsk",
            GuaranteeDate = DateTime.Today.AddYears(1).Date,
            Characteristics = "Test characteristics",
            Location = EquipmentLocationType.Belarus,
            Comment = "Test comment",
            Approver = new EquipmentUser
            {
                UserId = 1,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "email@CustomerName-software.com"
            },
            Assigns =
            [
                new EquipmentAssign
                {
                    Id = 4,
                    AssignedToUserId = 2,
                    EquipmentId = equipmentId,
                    IssueDate = new DateTime(2023, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReturnDate = new DateTime(2023, 7, 2, 0, 0, 0, DateTimeKind.Utc)
                }
            ]
        };

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(equipment);

        var request = new DeleteEquipmentHistoryCommand(equipmentId);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<InvalidDataAppException>();
        _equipmentAssignRepositoryMock.Verify(
            x => x.DeleteEquipmentAssignmentAsync(
                equipmentId,
                _cancellationToken),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenEquipmentContainActiveAssigns_ShouldDeleteActiveAssign()
    {
        // Arrange
        var equipmentId = 5;

        var assign = new EquipmentAssign
        {
            Id = 6,
            AssignedToUserId = 2,
            EquipmentId = equipmentId,
            IssueDate = new DateTime(2023, 7, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            TypeId = "Laptop",
            Location = EquipmentLocationType.Belarus,
            Characteristics = "Has keys",
            Name = "Laptop",
            PurchasePlace = "Market",
            SerialNumber = "0001",
            Assigns = [assign],
            Approver = new EquipmentUser
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@CustomerName-software.com"
            }
        };

        var expectedEquipmentHolderDto = new EquipmentHolderDto
        {
            Id = assign.AssignedToUserId,
            IssueDate = assign.IssueDate,
            FirstName = "Lorem",
            LastName = "Ipsum"
        };

        var equipmentToDelete = equipment;
        equipmentToDelete.Approver = null;
        equipmentToDelete.ApproverId = 1;

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(equipment);
        _equipmentAssignRepositoryMock.Setup(x => x.DeleteEquipmentAssignmentAsync(
                assign.Id,
                _cancellationToken))
            .ReturnsAsync(assign);

        _mapperMock.Setup(x => x.Map<EquipmentHolderDto>(assign))
            .Returns(expectedEquipmentHolderDto);

        var request = new DeleteEquipmentHistoryCommand(equipmentId);

        // Act
        var result = await _handler.Handle(request, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedEquipmentHolderDto);
    }
}

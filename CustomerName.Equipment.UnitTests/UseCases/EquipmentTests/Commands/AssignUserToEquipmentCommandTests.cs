using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentAssign;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Commands.AssignUserToEquipment;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Exceptions;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Commands;

public class AssignUserToEquipmentCommandTests
{
    private readonly Mock<IIsEquipmentExistByIdDbQuery> _isEquipmentExistByIdDbQueryMock = new();
    private readonly Mock<IGetEquipmentUserByIdDbQuery> _getEquipmentUserByIdDbQueryMock= new();
    private readonly Mock<ICreateEquipmentAssignDbQuery> _createEquipmentAssignDbQueryMock= new();
    private readonly Mock<IGetAssignsReturnDatesByEquipmentIdDbQuery> _getAssignsReturnDatesByEquipmentIdDbQueryMock= new();

    private readonly CancellationToken _cancellationToken;
    private readonly AssignUserToEquipmentCommandHandler _handler;

    public AssignUserToEquipmentCommandTests()
    {
        _cancellationToken = CancellationToken.None;
        _handler = new(
            _isEquipmentExistByIdDbQueryMock.Object,
            _getEquipmentUserByIdDbQueryMock.Object,
            _createEquipmentAssignDbQueryMock.Object,
            _getAssignsReturnDatesByEquipmentIdDbQueryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEquipmentDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var equipmentId = 1;
        const int userId = 5;
        var issueDate = DateTime.UtcNow.Date;

        var request = new AssignUserToEquipmentCommand(
            equipmentId,
            userId,
            issueDate);

        _isEquipmentExistByIdDbQueryMock.Setup(x => x.ExecuteAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<EntityNotFoundException>()
            .WithMessage(ExceptionConstants.EquipmentNotFound);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var equipmentId = 2;
        const int userId = 5;
        var issueDate = DateTime.UtcNow.Date;

        var request = new AssignUserToEquipmentCommand(
            equipmentId,
            userId,
            issueDate);

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            Name = Guid.NewGuid().ToString(),
            Characteristics = Guid.NewGuid().ToString(),
            PurchasePlace = Guid.NewGuid().ToString(),
            SerialNumber = Guid.NewGuid().ToString()
        };

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(equipmentId, _cancellationToken))
            .ReturnsAsync(true);

        _getEquipmentUserByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(userId,_cancellationToken))
            .ReturnsAsync((EquipmentUser?)null);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<EntityNotFoundException>()
            .WithMessage(ExceptionConstants.UserNotFound);
    }

    [Fact]
    public async Task Handle_WhenEquipmentAssignHasNoReturnDate_ShouldThrowEntityConflictException()
    {
        // Arrange
        var equipmentId = 3;
        const int userId = 5;
        var issueDate = DateTime.UtcNow.Date;

        var request = new AssignUserToEquipmentCommand(
            equipmentId,
            userId,
            issueDate);

        var user = new EquipmentUser
        {
            UserId = userId,
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}CustomerName-software.com"
        };

        var equipmentAssignsReturnDates = new DateTime?[]
        {
            null
        };

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(equipmentId, _cancellationToken))
            .ReturnsAsync(true);

        _getEquipmentUserByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(userId,_cancellationToken))
            .ReturnsAsync(user);

        _getAssignsReturnDatesByEquipmentIdDbQueryMock
            .Setup(x => x.ExecuteAsync(equipmentId, _cancellationToken))
            .ReturnsAsync(equipmentAssignsReturnDates);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<EntityConflictException>()
            .WithMessage(ExceptionConstants.EquipmentAlreadyAssigned);
    }

    [Fact]
    public async Task Handle_WhenEquipmentAssignsMaxReturnDateMoreThanRequestIssueDate_ShouldThrowEntityConflictException()
    {
        // Arrange
        var equipmentId = 3;
        const int userId = 5;
        var issueDate = DateTime.UtcNow.Date;
        var returnDate = DateTime.UtcNow.Date.AddDays(+1);

        var request = new AssignUserToEquipmentCommand(
            equipmentId,
            userId,
            issueDate);

        var user = new EquipmentUser
        {
            UserId = userId,
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}CustomerName-software.com"
        };

        var equipmentAssignsReturnDates = new DateTime?[]
        {
            returnDate
        };

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(equipmentId, _cancellationToken))
            .ReturnsAsync(true);

        _getEquipmentUserByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(userId,_cancellationToken))
            .ReturnsAsync(user);

        _getAssignsReturnDatesByEquipmentIdDbQueryMock
            .Setup(x => x.ExecuteAsync(equipmentId, _cancellationToken))
            .ReturnsAsync(equipmentAssignsReturnDates);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<EntityConflictException>()
            .WithMessage(ExceptionConstants.EquipmentAlreadyAssigned);
    }



    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    public async Task Handle_WhenCommandIsValid_ShouldAssignEquipment(int assignsCount)
    {
        // Arrange
        var equipmentId = 4;
        const int userId = 5;
        var issueDate = DateTime.UtcNow.Date;


        var request = new AssignUserToEquipmentCommand(
            equipmentId,
            userId,
            issueDate);

        var user = new EquipmentUser
        {
            UserId = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@CustomerName-software.com"
        };

        var equipmentAssignsReturnDates = new DateTime?[assignsCount];
        for (var i = 0; i < assignsCount; i++)
        {
            equipmentAssignsReturnDates[i] = issueDate.AddDays(-i);
        }

        var createdAssignment = new CreateEquipmentAssignResponse(5, issueDate);

        var expectedDto = new EquipmentHolderDto
        {
            Id = userId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DepartmentId = user.DepartmentId,
            IssueDate = createdAssignment.IssueDate,
            ReturnDate = null
        };

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(equipmentId, _cancellationToken))
            .ReturnsAsync(true);

        _getEquipmentUserByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(userId,_cancellationToken))
            .ReturnsAsync(user);

        _getAssignsReturnDatesByEquipmentIdDbQueryMock
            .Setup(x => x.ExecuteAsync(equipmentId, _cancellationToken))
            .ReturnsAsync(equipmentAssignsReturnDates);

        _createEquipmentAssignDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<EquipmentAssign>(), _cancellationToken))
            .ReturnsAsync(createdAssignment);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedDto);
        _createEquipmentAssignDbQueryMock
            .Verify(x => x.ExecuteAsync(
                It.Is<EquipmentAssign>(assign =>
                    assign.EquipmentId == equipmentId &&
                    assign.AssignedToUserId == userId),
                _cancellationToken), Times.Once);
    }
}

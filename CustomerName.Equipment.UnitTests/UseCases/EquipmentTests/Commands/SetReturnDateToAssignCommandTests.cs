using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetAssignsByEquipmentId;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.SetReturnDateToAssign;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Commands.PopulateReturnDate;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Framework.Exceptions;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Commands;

public class SetReturnDateToAssignCommandTests
{
    private readonly Mock<IIsEquipmentExistByIdDbQuery> _isEquipmentExistByIdDbQueryMock=new();
    private readonly Mock<IGetAssignsByEquipmentIdDbQuery> _getAssignsByEquipmentIdDbQueryMock=new();
    private readonly Mock<ISetReturnDateToAssignDbQuery> _setReturnDateToAssignDbQueryMock=new();
    private readonly Mock<IGetAssignHolderByAssignIdDbQuery> _getAssignHolderByAssignIdDbQueryMock=new();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private readonly SetReturnDateToAssignCommandHandler _handler;

    public SetReturnDateToAssignCommandTests()
    {
        _handler = new SetReturnDateToAssignCommandHandler(
            _isEquipmentExistByIdDbQueryMock.Object,
            _getAssignsByEquipmentIdDbQueryMock.Object,
            _setReturnDateToAssignDbQueryMock.Object,
            _getAssignHolderByAssignIdDbQueryMock.Object);
    }

    [Fact]
    public async Task SetReturnDateToAssign_WhenEquipmentDoesNotExist_ThrowEntityNotFoundException()
    {
        // Arrange
        var populateReturnDateCommand = new SetReturnDateToAssignCommand(
            8,
            new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Utc));

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<int>(),_cancellationToken))
            .ReturnsAsync(false);

        // Act
        var act = () => _handler.Handle(
            populateReturnDateCommand,
            _cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<EntityNotFoundException>()
            .WithMessage(ExceptionConstants.EquipmentNotFound);
    }

    [Fact]
    public async Task SetReturnDateToAssign_WhenEquipmentIsNotAssigned_ThrowInvalidDataAppException()
    {
        // Arrange
        var populateReturnDateCommand = new SetReturnDateToAssignCommand(
            8,
            new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Utc));

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<int>(),_cancellationToken))
            .ReturnsAsync(true);

        _getAssignsByEquipmentIdDbQueryMock
            .Setup(x=>x.ExecuteAsync(populateReturnDateCommand.EquipmentId,_cancellationToken))
            .ReturnsAsync([]);

        // Act
        var act = () => _handler.Handle(
            populateReturnDateCommand,
            _cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidDataAppException>()
            .WithMessage(ExceptionConstants.EquipmentWasNeverAssigned);
    }

    [Fact]
    public async Task SetReturnDateToAssign_WhenEquipmentHasSeveralAssignsWithoutReturnDate_ThrowInvalidDataAppException()
    {
        // Arrange
        var populateReturnDateCommand = new SetReturnDateToAssignCommand(
            8,
            new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Utc));

        var assignsWithoutReturnDate = Enumerable.Repeat(new EquipmentAssign
        {
            ReturnDate = null
        }, 2).ToList();

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<int>(),_cancellationToken))
            .ReturnsAsync(true);

        _getAssignsByEquipmentIdDbQueryMock
            .Setup(x=>x.ExecuteAsync(populateReturnDateCommand.EquipmentId,_cancellationToken))
            .ReturnsAsync(assignsWithoutReturnDate);

        // Act
        var act = () => _handler.Handle(
            populateReturnDateCommand,
            _cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidDataAppException>()
            .WithMessage(ExceptionConstants.SeveralNullDatesForEquipment);
    }

    [Fact]
    public async Task SetReturnDateToAssign_WhenReturnDateAlreadyExists_ShouldThrowEntityConflictException()
    {
        // Arrange
        var populateReturnDateCommand = new SetReturnDateToAssignCommand(
            12,
            new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Utc));

        var assigns = new List<EquipmentAssign>
        {
            new()
            {
                ReturnDate = new DateTime(2022, 6, 12, 0, 0, 0, DateTimeKind.Utc),
            }
        };

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<int>(),_cancellationToken))
            .ReturnsAsync(true);

        _getAssignsByEquipmentIdDbQueryMock
            .Setup(x=>x.ExecuteAsync(populateReturnDateCommand.EquipmentId,_cancellationToken))
            .ReturnsAsync(assigns);

        // Act
        var act = () => _handler.Handle(
            populateReturnDateCommand,
            _cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<EntityConflictException>()
            .WithMessage(ExceptionConstants.ExistingReturnDateForLastAssignment);
    }

    [Fact]
    public async Task SetReturnDateToAssign_WhenReturnDateIsLessThanIssueDate_ThrowInvalidDataAppException()
    {
        // Arrange
        var returnDate = new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Utc);

        var populateReturnDateCommand = new SetReturnDateToAssignCommand(
            12,
            returnDate);

        var assigns = new List<EquipmentAssign>
        {
            new()
            {
                IssueDate = returnDate.AddDays(1),
                ReturnDate = null,
            }
        };

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<int>(),_cancellationToken))
            .ReturnsAsync(true);

        _getAssignsByEquipmentIdDbQueryMock
            .Setup(x=>x.ExecuteAsync(populateReturnDateCommand.EquipmentId,_cancellationToken))
            .ReturnsAsync(assigns);

        // Act
        var act = () => _handler.Handle(
            populateReturnDateCommand,
            _cancellationToken);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidDataAppException>()
            .WithMessage(ExceptionConstants.ReturnDateLowerThanIssueDate);
    }

    [Fact]
    public async Task SetReturnDateToAssign_WhenDataIsValid_ReturnUpdatedEquipmentModel()
    {
        // Arrange
        var returnDate = new DateTime(2023, 7, 12, 0, 0, 0, DateTimeKind.Utc);

        var populateReturnDateCommand = new SetReturnDateToAssignCommand(
            12,
            returnDate);

        var assigns = new List<EquipmentAssign>
        {
            new()
            {
                IssueDate = returnDate.AddDays(-1),
                ReturnDate = null,
            }
        };

        var expectedHolder = new ActiveHolderDto
        {
            Id = 1,
            FirstName = "Test",
            LastName = "User",
            DepartmentId = "QA",
            IssueDate = assigns[0].IssueDate,
            ReturnDate = returnDate
        };

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<int>(),_cancellationToken))
            .ReturnsAsync(true);

        _getAssignsByEquipmentIdDbQueryMock
            .Setup(x=>x.ExecuteAsync(populateReturnDateCommand.EquipmentId,_cancellationToken))
            .ReturnsAsync(assigns);

        _setReturnDateToAssignDbQueryMock
            .Setup(x=>x.ExecuteAsync(It.IsAny<SetReturnDateToAssignRequest>(),_cancellationToken))
            .Returns(Task.CompletedTask);

        _getAssignHolderByAssignIdDbQueryMock
            .Setup(x=>x.ExecuteAsync(It.IsAny<int>(),_cancellationToken))
            .ReturnsAsync(expectedHolder);

        // Act
        var act = await _handler.Handle(
            populateReturnDateCommand,
            _cancellationToken);

        // Assert
        act.Should().BeEquivalentTo(expectedHolder);

        _setReturnDateToAssignDbQueryMock
            .Verify(y=> y.ExecuteAsync(It.IsAny<SetReturnDateToAssignRequest>(), _cancellationToken), Times.Once);
    }

}

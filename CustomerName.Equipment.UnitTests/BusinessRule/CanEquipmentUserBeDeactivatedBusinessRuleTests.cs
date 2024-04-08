using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;

namespace CustomerName.Portal.Equipment.UnitTests.BusinessRule;

public class CanEquipmentUserBeDeactivatedBusinessRuleTests
{
    private readonly Mock<IIsUserWithActiveEquipmentAssignmentsDbQuery> _isUserWithActiveEquipmentAssignmentsDbQueryMock = new();
    private readonly Mock<IIsUserApproverOfEquipmentDbQuery> _isUserApproverOfEquipmentDbQueryMock = new();

    private readonly CanEquipmentUserBeDeactivatedBusinessRule _canEquipmentUserBeDeactivatedBusinessRule;

    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public CanEquipmentUserBeDeactivatedBusinessRuleTests()
    {
        _canEquipmentUserBeDeactivatedBusinessRule = new CanEquipmentUserBeDeactivatedBusinessRule(
            _isUserWithActiveEquipmentAssignmentsDbQueryMock.Object,
            _isUserApproverOfEquipmentDbQueryMock.Object);
    }

    [Fact]
    public async Task IsCorrespondingToBusiness_WhenUserHasNoActiveEquipmentAssignmentsAndUserIsNotApproverOfEquipment_ShouldReturnTrue()
    {
        //Arrange
        const int userId = 1;

        _isUserWithActiveEquipmentAssignmentsDbQueryMock.Setup(x =>
                x.ExecuteAsync(userId, _cancellationToken))
            .ReturnsAsync(false);

        _isUserApproverOfEquipmentDbQueryMock.Setup(x =>
                x.ExecuteAsync(userId, _cancellationToken))
            .ReturnsAsync(false);

        //Act
        var actualResult = await _canEquipmentUserBeDeactivatedBusinessRule.IsCorrespondsToBusiness(userId, _cancellationToken);

        //Assert
        actualResult.Should().BeTrue();
    }

    [Fact]
    public async Task IsCorrespondingToBusiness_WhenUserHasActiveEquipmentAssignments_ShouldReturnFalse()
    {
        //Arrange
        const int userId = 1;

        _isUserWithActiveEquipmentAssignmentsDbQueryMock.Setup(x =>
                x.ExecuteAsync(userId, _cancellationToken))
            .ReturnsAsync(true);

        _isUserApproverOfEquipmentDbQueryMock.Setup(x =>
                x.ExecuteAsync(userId, _cancellationToken))
            .ReturnsAsync(false);

        //Act
        var actualResult = await _canEquipmentUserBeDeactivatedBusinessRule.IsCorrespondsToBusiness(userId, _cancellationToken);

        //Assert
        actualResult.Should().BeFalse();
    }

    [Fact]
    public async Task IsCorrespondingToBusiness_WhenUserIsApproverOfEquipment_ShouldReturnFalse()
    {
        //Arrange
        const int userId = 1;

        _isUserWithActiveEquipmentAssignmentsDbQueryMock.Setup(x =>
                x.ExecuteAsync(userId, _cancellationToken))
            .ReturnsAsync(false);

        _isUserApproverOfEquipmentDbQueryMock.Setup(x =>
                x.ExecuteAsync(userId, _cancellationToken))
            .ReturnsAsync(true);

        //Act
        var actualResult = await _canEquipmentUserBeDeactivatedBusinessRule.IsCorrespondsToBusiness(userId, _cancellationToken);

        //Assert
        actualResult.Should().BeFalse();
    }
}

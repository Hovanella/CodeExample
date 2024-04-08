using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsEquipmentUserFromDepartment;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.UnitTests.BusinessRule;

public class IsAllowedToGetUserEquipmentBusinessRuleTests
{
    private readonly Mock<IAuthenticatedUserContext> _authenticatedUserContextMock = new();
    private readonly Mock<IIsEquipmentUserFromDepartmentDbQuery> _isEquipmentUserFromDepartmentDbQueryMock= new();

    [Theory]
    [InlineData(RoleType.SystemAdministrator)]
    [InlineData(RoleType.CTO)]
    [InlineData(RoleType.CEO)]
    public async Task IsCorrespondingToBusiness_WhenRoleCanGetAllEquipment_ShouldReturnTrue(RoleType roleType)
    {
        //Arrange
        _authenticatedUserContextMock
            .Setup(x => x.Role)
            .Returns(roleType);

        var isAllowedToGetUserEquipmentBusinessRule = new IsAllowedToGetUserEquipmentBusinessRule(
            _authenticatedUserContextMock.Object,
            _isEquipmentUserFromDepartmentDbQueryMock.Object);

        //Act
        var result = await isAllowedToGetUserEquipmentBusinessRule.IsCorrespondsToBusiness(1, CancellationToken.None);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsCorrespondingToBusiness_WhenHeadOfDepartmentTryToGetEquipmentOfUserFromAnotherDepartment_ShouldReturnFalse()
    {
        //Arrange
        _authenticatedUserContextMock
            .Setup(x => x.Role)
            .Returns(RoleType.HeadOfDepartment);

        _authenticatedUserContextMock
            .Setup(x => x.DepartmentId)
            .Returns("Net");

        _isEquipmentUserFromDepartmentDbQueryMock
            .Setup(x => x.IsUserFromDepartment(It.IsAny<int>(),It.IsAny<string>(), CancellationToken.None ))
            .ReturnsAsync(false);

        var isAllowedToGetUserEquipmentBusinessRule = new IsAllowedToGetUserEquipmentBusinessRule(_authenticatedUserContextMock.Object,
            _isEquipmentUserFromDepartmentDbQueryMock.Object);

        //Act
        var result = await isAllowedToGetUserEquipmentBusinessRule.IsCorrespondsToBusiness(1, CancellationToken.None);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsCorrespondingToBusiness_WhenHeadOfDepartmentTryToGetEquipmentOfUserFromSameDepartment_ShouldReturnFalse()
    {
        //Arrange
        _authenticatedUserContextMock
            .Setup(x => x.Role)
            .Returns(RoleType.HeadOfDepartment);

        _authenticatedUserContextMock
            .Setup(x => x.DepartmentId)
            .Returns("Net");

        _isEquipmentUserFromDepartmentDbQueryMock
            .Setup(x => x.IsUserFromDepartment(It.IsAny<int>(),It.IsAny<string>(), CancellationToken.None ))
            .ReturnsAsync(true);

        var isAllowedToGetUserEquipmentBusinessRule = new IsAllowedToGetUserEquipmentBusinessRule(
            _authenticatedUserContextMock.Object,
            _isEquipmentUserFromDepartmentDbQueryMock.Object);

        //Act
        var result = await isAllowedToGetUserEquipmentBusinessRule.IsCorrespondsToBusiness(1, CancellationToken.None);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsCorrespondingToBusiness_WhenEmployeeTryToGetEquipmentFromUserWithAnotherId_ShouldReturnFalse()
    {
        //Arrange
        _authenticatedUserContextMock
            .Setup(x => x.Role)
            .Returns(RoleType.Employee);

        _authenticatedUserContextMock
            .Setup(x => x.UserId)
            .Returns(2);

        var isAllowedToGetUserEquipmentBusinessRule = new IsAllowedToGetUserEquipmentBusinessRule(
            _authenticatedUserContextMock.Object,
            _isEquipmentUserFromDepartmentDbQueryMock.Object);

        //Act
        var result = await isAllowedToGetUserEquipmentBusinessRule.IsCorrespondsToBusiness(1, CancellationToken.None);

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsCorrespondingToBusiness_WhenEmployeeTryToGetEquipmentFromUserWithSameId_ShouldReturnTrue()
    {
        //Arrange
        _authenticatedUserContextMock
            .Setup(x => x.Role)
            .Returns(RoleType.Employee);

        _authenticatedUserContextMock
            .Setup(x => x.UserId)
            .Returns(1);

        var isAllowedToGetUserEquipmentBusinessRule = new IsAllowedToGetUserEquipmentBusinessRule(
            _authenticatedUserContextMock.Object,
            _isEquipmentUserFromDepartmentDbQueryMock.Object);

        //Act
        var result = await isAllowedToGetUserEquipmentBusinessRule.IsCorrespondsToBusiness(1, CancellationToken.None);

        //Assert
        result.Should().BeTrue();
    }


}

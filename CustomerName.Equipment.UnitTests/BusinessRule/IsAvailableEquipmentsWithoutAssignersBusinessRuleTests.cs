using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UnitTests.BusinessRule;

public class IsAvailableEquipmentsWithoutAssignersBusinessRuleTests
{
    private readonly Mock<IAuthenticatedUserContext> _authenticatedUserContext = new();

    [Theory]
    [InlineData(RoleType.CTO, true)]
    [InlineData(RoleType.CEO, true)]
    [InlineData(RoleType.Employee, false)]
    [InlineData(RoleType.SystemAdministrator, true)]
    public async Task IsCorrespondsToBusiness_WhenRoleSupported_ShouldReturnExpectedValue(RoleType roleType,bool expectedResult )
    {
        //Arrange

        var identityContractMock = new Mock<IIdentityContract>();
        var isAvailableEquipmentsWithoutAssignersBusinessRule = new IsAvailableEquipmentsWithoutAssignersBusinessRule(identityContractMock.Object);

        _authenticatedUserContext.SetupGet(x => x.Role).Returns(roleType);

        //Act
        var result = await isAvailableEquipmentsWithoutAssignersBusinessRule.IsCorrespondsToBusiness(_authenticatedUserContext.Object, CancellationToken.None);

        //Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task IsCorrespondsToBusiness_WhenAuthedAsHeadOfDepartmentAndIsAvailableToGetEquipmentsWithoutAssigners()
    {
        var identityContractMock = new Mock<IIdentityContract>();
        var isAvailableEquipmentsWithoutAssignersBusinessRule = new IsAvailableEquipmentsWithoutAssignersBusinessRule(identityContractMock.Object);

        _authenticatedUserContext.SetupGet(x => x.Role).Returns(RoleType.HeadOfDepartment);

        identityContractMock
            .Setup(x => x.IsAvailableEquipmentsWithoutAssignersAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(true);

        //Act
        var result = await isAvailableEquipmentsWithoutAssignersBusinessRule.IsCorrespondsToBusiness(_authenticatedUserContext.Object, CancellationToken.None);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task IsCorrespondsToBusiness_WhenAuthedAsHeadOfDepartmentAndIsNotAvailableToGetEquipmentsWithoutAssigners()
    {
        var identityContractMock = new Mock<IIdentityContract>();
        var isAvailableEquipmentsWithoutAssignersBusinessRule = new IsAvailableEquipmentsWithoutAssignersBusinessRule(identityContractMock.Object);

        _authenticatedUserContext.SetupGet(x => x.Role).Returns(RoleType.HeadOfDepartment);

        identityContractMock
            .Setup(x => x.IsAvailableEquipmentsWithoutAssignersAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(false);

        //Act
        var result = await isAvailableEquipmentsWithoutAssignersBusinessRule.IsCorrespondsToBusiness(_authenticatedUserContext.Object, CancellationToken.None);

        //Assert
        result.Should().Be(false);
    }

    [Fact]
    public void IsCorrespondsToBusiness_WhenRoleNotSupported_ShouldThrowApplicationException()
    {
        //Arrange
        var identityContractMock = new Mock<IIdentityContract>();
        var isAvailableEquipmentsWithoutAssignersBusinessRule = new IsAvailableEquipmentsWithoutAssignersBusinessRule(identityContractMock.Object);

        _authenticatedUserContext.SetupGet(x => x.Role).Returns((RoleType) 100);

        //Act
        var act = () => isAvailableEquipmentsWithoutAssignersBusinessRule.IsCorrespondsToBusiness(_authenticatedUserContext.Object, CancellationToken.None).Wait();

        //Assert
        act.Should().Throw<ApplicationException>();
    }
}

using CustomerName.Portal.Equipment.UseCases.Commands.AssignUserToEquipment;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Validators;

public class AssignUserToEquipmentCommandValidatorTests
{
    private readonly AssignUserToEquipmentCommandValidator _validator = new();

    [Theory]
    [MemberData(nameof(GetInvalidAssignUserToEquipmentCommandParams))]
    public void Validate_WhenCommandIsInvalid_ShouldReturnInvalidResult(
        int equipmentId,
        DateTime issueDate)
    {
        // Arrange
        const int userId = 1;

        var command = new AssignUserToEquipmentCommand(
            equipmentId,
            userId,
            issueDate);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetValidAssignUserToEquipmentCommandParams))]
    public void Validate_WhenCommandIsValid_ShouldReturnValidResult(
        int equipmentId,
        DateTime issueDate)
    {
        // Arrange
        const int userId = 1;

        var command = new AssignUserToEquipmentCommand(
            equipmentId,
            userId,
            issueDate);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidAssignUserToEquipmentCommandParams()
    {
        yield return
        [
            -1,
            new DateTime(2023, 7, 22, 0, 0, 0, DateTimeKind.Utc)
        ];
        yield return
        [
            1,
            new DateTime(2023, 7, 22, 0, 0, 1, DateTimeKind.Utc)
        ];
        yield return
        [
            -1,
            new DateTime(2023, 7, 22, 11, 56, 12, DateTimeKind.Utc)
        ];
    }

    public static IEnumerable<object[]> GetValidAssignUserToEquipmentCommandParams()
    {
        yield return
        [
            1,
            new DateTime(2023, 7, 22, 0, 0, 0, DateTimeKind.Utc)
        ];
        yield return
        [
            1,
            DateTime.UtcNow.Date
        ];
    }
}

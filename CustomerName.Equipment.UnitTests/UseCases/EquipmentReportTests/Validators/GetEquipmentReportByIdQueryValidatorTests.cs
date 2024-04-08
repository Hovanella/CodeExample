using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportById;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentReportTests.Validators;

public class GetEquipmentReportByIdQueryValidatorTests
{
    private readonly GetEquipmentReportByIdQueryValidator _validator = new();

    [Theory]
    [MemberData(nameof(GetInvalidIds))]
    public void Validate_WhenQueryIsInvalid_ShouldReturnInvalidResult(int id)
    {
        //Arrange
        var query = new GetEquipmentReportByIdQuery(id);

        //Act
        var result = _validator.Validate(query);

        //Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetValidIds))]
    public void Validate_WhenQueryValid_ShouldReturnValidResult(int id)
    {
        //Arrange
        var query = new GetEquipmentReportByIdQuery(id);

        //Act
        var result = _validator.Validate(query);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetValidIds()
    {
        yield return [1];
        yield return [101];
    }

    public static IEnumerable<object[]> GetInvalidIds()
    {
        yield return [0];
        yield return [-5];
    }
}

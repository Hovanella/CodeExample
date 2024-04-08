using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportRelevancePeriods;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentReportTests.Validators;

public class GetEquipmentReportRelevancePeriodsQueryValidatorTests
{
    private readonly GetEquipmentReportRelevancePeriodsQueryValidator _validator = new();

    [Theory]
    [MemberData(nameof(GetInvalidSerialNumbers))]
    public void Validate_WhenQueryIsInvalid_ShouldReturnInvalidResult(string serialNumber)
    {
        //Arrange
        var query = new GetEquipmentReportRelevancePeriodsQuery(serialNumber);

        //Act
        var result = _validator.Validate(query);

        //Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetValidSerialNumbers))]
    public void Validate_WhenQueryValid_ShouldReturnValidResult(string serialNumber)
    {
        //Arrange
        var query = new GetEquipmentReportRelevancePeriodsQuery(serialNumber);

        //Act
        var result = _validator.Validate(query);

        //Assert
        result.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetValidSerialNumbers()
    {
        yield return ["SKVKAFA"];
        yield return ["PASMVQW"];
    }

    public static IEnumerable<object[]> GetInvalidSerialNumbers()
    {
        yield return [""];
    }
}

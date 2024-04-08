using CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Validators;

public class UpdateEquipmentCommandValidatorTests
{
    private readonly UpdateEquipmentCommandValidator _validator = new();
    private readonly UpdateEquipmentCommand _validCommand = new(
        1,
        "Valid Name",
        "Laptop",
        EquipmentLocationType.Belarus,
        "Valid SerialNumber",
        5999.99m,
        MoneyCurrencyType.Byn,
        1999.99m,
        new DateTime(2023, 05, 22, 0, 0, 0, DateTimeKind.Utc),
        "Valid PurchasePlace",
        new DateTime(2026, 05, 22, 0, 0, 0, DateTimeKind.Utc),
        "Valid Characteristics",
        "Valid Comment",
        "DSAMVAS123ASKD123",
        1);

    [Fact]
    public void Validate_WhenIdIsNotValid_ShouldReturnInvalidResult()
    {
        // Arrange
        var command = _validCommand with { Id = -1 };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-22)]
    public void Validate_WhenApproverIdIsNotValid_ShouldReturnInvalidResult(int approverId)
    {
        // Arrange
        var command = _validCommand with { ApproverId = approverId };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenCharacteristicsIsNotValid_ShouldReturnInvalidResult()
    {
        // Arrange
        var command = _validCommand with { Characteristics = string.Empty };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetInvalidComments))]
    public void Validate_WhenCommentIsNotValid_ShouldReturnInvalidResult(string? comment)
    {
        // Arrange
        var command = _validCommand with { Comment = comment };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetInvalidGuaranteeDates))]
    public void Validate_WhenGuaranteeDateIsNotValid_ShouldReturnInvalidResult(DateTime guaranteeDate)
    {
        // Arrange
        var command = _validCommand with { GuaranteeDate = guaranteeDate };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetValidInvoiceNumber))]
    public void Validate_WhenInvoiceNumberIsValid_ShouldReturnValidResult(string invoiceNumber)
    {
        // Arrange
        var command = _validCommand with { InvoiceNumber = invoiceNumber };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidInvoiceNumber))]
    public void Validate_WhenInvoiceNumberIsNotValid_ShouldReturnInvalidResult(string invoiceNumber)
    {
        // Arrange
        var command = _validCommand with { InvoiceNumber = invoiceNumber };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData((EquipmentLocationType)(-1))]
    [InlineData((EquipmentLocationType)3)]
    [InlineData((EquipmentLocationType)4)]
    public void Validate_WhenLocationIsNotValid_ShouldReturnInvalidResult(EquipmentLocationType locationType)
    {
        // Arrange
        var command = _validCommand with { Location = locationType };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetInvalidNames))]
    public void Validate_WhenNameIsNotValid_ShouldReturnInvalidResult(string name)
    {
        // Arrange
        var command = _validCommand with { Name = name };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetInvalidPurchaseDates))]
    public void Validate_WhenPurchaseDateIsNotValid_ShouldReturnInvalidResult(DateTime purchaseDate)
    {
        // Arrange
        var command = _validCommand with { PurchaseDate = purchaseDate };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetInvalidPurchasePlaces))]
    public void Validate_WhenPurchasePlaceIsNotValid_ShouldReturnInvalidResult(string purchasePlace)
    {
        // Arrange
        var command = _validCommand with { PurchasePlace = purchasePlace };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(-55.99)]
    [InlineData(-0.01)]
    [InlineData(0.001)]
    [InlineData(0.000064)]
    [InlineData(1E-13)]
    [InlineData(15E-3)]
    public void Validate_WhenPurchasePriceIsNotValid_ShouldReturnInvalidResult(decimal purchasePrice)
    {
        // Arrange
        var command = _validCommand with { PurchasePrice = purchasePrice };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

    }

    [Theory]
    [InlineData(-55.99)]
    [InlineData(-0.01)]
    [InlineData(0.001)]
    [InlineData(0.000064)]
    [InlineData(1E-13)]
    [InlineData(15E-3)]
    public void Validate_WhenPurchasePriceUsdIsNotValid_ShouldReturnInvalidResult(decimal purchasePriceUsd)
    {
        // Arrange
        var command = _validCommand with { PurchasePriceUsd = purchasePriceUsd };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();

    }

    [Theory]
    [MemberData(nameof(GetInvalidSerialNumbers))]
    public void Validate_WhenSerialNumberIsNotValid_ShouldReturnInvalidResult(string serialNumber)
    {
        // Arrange
        var command = _validCommand with { SerialNumber = serialNumber };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WhenCommandIsValid_ShouldReturnValidResult()
    {
        // Arrange
        var command = _validCommand;

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidComments()
    {
        yield return [new string('a', 1001)];
    }

    public static IEnumerable<object[]> GetInvalidInvoiceNumber()
    {
        yield return [new string('x', 251)];
        yield return [new string('b', 1)];
        yield return [new string("   ***---")];
    }

    public static IEnumerable<object[]> GetValidInvoiceNumber()
    {
        yield return [new string('b', 2)];
        yield return [new string('s', 250)];
    }

    public static IEnumerable<object[]> GetInvalidGuaranteeDates()
    {
        yield return [new DateTime(2026, 07, 07, 11, 29, 55, DateTimeKind.Utc)];
        yield return [new DateTime(2026, 07, 07, 0, 0, 1, DateTimeKind.Utc)];
    }

    public static IEnumerable<object[]> GetInvalidNames()
    {
        yield return [string.Empty];
        yield return [new string('a', 251)];
    }

    public static IEnumerable<object[]> GetInvalidPurchaseDates()
    {
        yield return [new DateTime(2022, 12, 16, 11, 29, 55, DateTimeKind.Utc)];
        yield return [new DateTime(2022, 6, 22, 0, 0, 1, DateTimeKind.Utc)];
    }

    public static IEnumerable<object[]> GetInvalidPurchasePlaces()
    {
        yield return [string.Empty];
        yield return [new string('Y', 251)];
    }

    public static IEnumerable<object[]> GetInvalidSerialNumbers()
    {
        yield return [string.Empty];
        yield return [new string('u', 251)];
    }
}

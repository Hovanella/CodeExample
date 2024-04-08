using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipment;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Commands;

public class CreateEquipmentCommandTests
{
    private readonly Mock<IIdentityContract> _identityContractMock = new();
    private readonly Mock<IIsEquipmentTypeExistDbQuery> _isEquipmentTypeExistDbQueryMock = new();
    private readonly Mock<IIsEquipmentUserExistDbQuery> _isEquipmentUserExistDbQueryMock = new();
    private readonly Mock<IIsSerialNumberExistingDbQuery> _isSerialNumberExistingDbQueryMock = new();
    private readonly Mock<IGetEquipmentApproverAndTypeByIdDbQuery> _getEquipmentApproverAndTypeByIdDbQueryMock = new();
    private readonly Mock<ICreateEquipmentDbQuery> _createEquipmentDbQueryMock = new();
    private readonly Mock<IEquipmentToEquipmentDtoMapper> _equipmentToEquipmentDtoMapperMock = new();
    private readonly Mock<ICreateEquipmentCommandToEquipmentMapper> _createEquipmentCommandToEquipmentMapperMock = new();

    private readonly CancellationToken _cancellationToken;
    private readonly CreateEquipmentCommandHandler _handler;

    public CreateEquipmentCommandTests()
    {
        _cancellationToken = CancellationToken.None;
        _handler = new CreateEquipmentCommandHandler(
            _identityContractMock.Object,
            _isEquipmentTypeExistDbQueryMock.Object,
            _isEquipmentUserExistDbQueryMock.Object,
            _getEquipmentApproverAndTypeByIdDbQueryMock.Object,
            _createEquipmentDbQueryMock.Object,
            _equipmentToEquipmentDtoMapperMock.Object,
            _createEquipmentCommandToEquipmentMapperMock.Object,
            _isSerialNumberExistingDbQueryMock.Object);
    }

    [Fact]
    public async Task Handle_ApproverDoesNotHavePermission_ShouldThrowActionNotAllowedException()
    {
        // Arrange
        const int approverId = 1;

        var request = new CreateEquipmentCommand(
            "Name",
            "Headset",
            EquipmentLocationType.Belarus,
            "ABC-CBA",
            3999.99m,
            MoneyCurrencyType.Byn,
            1999.99m,
            DateTime.UtcNow.Date,
            "PurchasePlace",
            DateTime.UtcNow.Date.AddYears(2),
            "Characteristics",
            "Comment",
            "IKSAJ123124SA",
            approverId);

        _isEquipmentUserExistDbQueryMock.Setup(x => x.ExecuteAsync(approverId, _cancellationToken))
                                        .ReturnsAsync(true);

        _identityContractMock.Setup(x => x.HasUserAnyRoleAsync(approverId, It.IsAny<List<RoleType>>(), _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<ActionNotAllowedException>()
            .WithMessage(ExceptionConstants.ApproverDoesNotHavePermission);

        _createEquipmentDbQueryMock.Verify(
            x => x.CreateEquipmentAsync(
                It.IsAny<Domain.Equipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateEquipment()
    {
        // Arrange
        const int userId = 1;

        var request = new CreateEquipmentCommand(
            "Name",
            "Headset",
            EquipmentLocationType.Belarus,
            "ABC-CBA",
            3999.99m,
            MoneyCurrencyType.Byn,
            1999.99m,
            DateTime.UtcNow.Date,
            "PurchasePlace",
            DateTime.UtcNow.Date.AddYears(2),
            "Characteristics",
            "Comment",
            "DWEVDVQ3214EDA",
            userId);

        var equipmentToAdd = new Domain.Equipment
        {
            Characteristics = request.Characteristics,
            Name = request.Name,
            PurchasePlace = request.PurchasePlace,
            SerialNumber = request.SerialNumber,
            ApproverId = request.ApproverId
        };

        var expectedEquipmentUser = new EquipmentUser
        {
            UserId = request.ApproverId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@CustomerName-software.com"
        };

        var expectedEquipment = new Domain.Equipment
        {
            Id = 1,
            ApproverId = request.ApproverId,
            Characteristics = equipmentToAdd.Characteristics,
            Name = equipmentToAdd.Name,
            PurchasePlace = equipmentToAdd.PurchasePlace,
            SerialNumber = equipmentToAdd.SerialNumber,
            Approver = expectedEquipmentUser,
            Type = new EquipmentType(request.TypeId, "Headset", "Headset")
        };

        var expectedEquipmentDto = new EquipmentDto
        {
            Id = expectedEquipment.Id,
            Characteristics = request.Characteristics,
            GuaranteeDate = request.GuaranteeDate,
            Location = request.Location,
            Name = request.Name,
            PurchaseDate = request.PurchaseDate,
            PurchasePlace = request.PurchasePlace,
            PurchasePrice = request.PurchasePrice,
            PurchasePriceUsd = request.PurchasePriceUsd,
            SerialNumber = request.SerialNumber,
            TypeId = request.TypeId,
            Approver = new ApproverDto
            {
                Id = request.ApproverId,
                FirstName = expectedEquipmentUser.FirstName,
                LastName = expectedEquipmentUser.LastName
            },
            Type = new EquipmentTypeDto(request.TypeId, "Headset", "Headset")
        };

        _identityContractMock.Setup(x => x.HasUserAnyRoleAsync(
                It.IsAny<int>(),
                It.IsAny<List<RoleType>>(),
                _cancellationToken))
            .ReturnsAsync(true);

        _createEquipmentCommandToEquipmentMapperMock.Setup(x => x.Map(request))
            .Returns(equipmentToAdd);

        _equipmentToEquipmentDtoMapperMock.Setup(x => x.Map(expectedEquipment))
            .Returns(expectedEquipmentDto);

        _createEquipmentDbQueryMock.Setup(x => x.CreateEquipmentAsync(
                equipmentToAdd,
                _cancellationToken))
            .ReturnsAsync(expectedEquipment);

        _getEquipmentApproverAndTypeByIdDbQueryMock.Setup(x => x.ExecuteAsync(
                equipmentToAdd.ApproverId,
                _cancellationToken))
            .ReturnsAsync(expectedEquipment);

        _isEquipmentUserExistDbQueryMock.Setup(x => x.ExecuteAsync(
                request.ApproverId,
                _cancellationToken))
            .ReturnsAsync(true);

        _isEquipmentTypeExistDbQueryMock.Setup(x => x.IsEquipmentTypeExist(
                request.TypeId,
                _cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedEquipmentDto);

        _identityContractMock.Verify(
            x => x.HasUserAnyRoleAsync(
                userId,
                It.IsAny<List<RoleType>>(),
                _cancellationToken),
            Times.Once);

        _createEquipmentDbQueryMock.Verify(
            x => x.CreateEquipmentAsync(
                equipmentToAdd,
                _cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ApproverDoesNotExists_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        const int approverId = 1;

        var request = new CreateEquipmentCommand(
            "Name",
            "Headset",
            EquipmentLocationType.Belarus,
            "ABC-CBA",
            3999.99m,
            MoneyCurrencyType.Byn,
            1999.99m,
            DateTime.UtcNow.Date,
            "PurchasePlace",
            DateTime.UtcNow.Date.AddYears(2),
            "Characteristics",
            "Comment",
            "DWEVDVQ3214EDA",
            approverId);

        _isEquipmentUserExistDbQueryMock.Setup(x => x.ExecuteAsync(
                approverId,
                _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<EntityNotFoundException>().WithMessage(ExceptionConstants.ApproverDoesNotExist);
    }
}

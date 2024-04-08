using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.IsSerialNumberExisting;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.UpdateEquipmentDbQuery;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Commands;

public class UpdateEquipmentCommandTests
{
    private readonly Mock<IUpdateEquipmentDbQuery> _updateEquipmentDbQueryMock = new();
    private readonly Mock<IIsEquipmentExistByIdDbQuery> _isEquipmentExistByIdDbQueryMock = new();
    private readonly Mock<IGetEquipmentApproverDbQuery> _getEquipmentApproverDbQueryMock = new();
    private readonly Mock<IGetEquipmentTypeByIdDbQuery> _getEquipmentTypeByIdDbQueryMock = new();
    private readonly Mock<IIsEquipmentTypeExistDbQuery> _isEquipmentTypeExistDbQueryMock = new();
    private readonly Mock<IIsSerialNumberExistingDbQuery> _isSerialNumberExistingDbQueryMock = new();

    private readonly IUpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper _updateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper = new UpdateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper();
    private readonly IUpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper _updateEquipmentDbQueryToUpdatedEquipmentDtoMapper = new UpdateEquipmentDbQueryToUpdatedEquipmentDtoMapper() ;

    private readonly Mock<IIdentityContract> _identityContractMock = new();

    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private readonly UpdateEquipmentCommandHandler _handler;

    public UpdateEquipmentCommandTests()
    {
        _handler = new UpdateEquipmentCommandHandler(
            _identityContractMock.Object,
            _isEquipmentTypeExistDbQueryMock.Object,
            _getEquipmentTypeByIdDbQueryMock.Object,
            _updateEquipmentDbQueryMock.Object,
            _updateEquipmentCommandToUpdateEquipmentDbQueryRequestMapper,
            _getEquipmentApproverDbQueryMock.Object,
            _isEquipmentExistByIdDbQueryMock.Object,
            _updateEquipmentDbQueryToUpdatedEquipmentDtoMapper,
            _isSerialNumberExistingDbQueryMock.Object
        );
    }

    [Fact]
    public async Task UpdateEquipmentAsync_WhenApproverDoesNotExists_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        const int approverId = 1;

        var request = new UpdateEquipmentCommand(
            1,
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

        _getEquipmentApproverDbQueryMock
            .Setup(x => x.ExecuteAsync(request.Id, _cancellationToken))
            .ReturnsAsync(null as EquipmentUser);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<EntityNotFoundException>()
            .WithMessage(ExceptionConstants.ApproverDoesNotExist);
    }

    [Fact]
    public async Task UpdateEquipmentAsync_WhenApproverDoesNotHavePermission_ShouldThrowActionNotAllowedException()
    {
        // Arrange
        const int approverId = 1;

        var request = new UpdateEquipmentCommand(
            1,
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

        _getEquipmentApproverDbQueryMock
            .Setup(x => x.ExecuteAsync(approverId, _cancellationToken))
            .ReturnsAsync(new EquipmentUser
            {
                FirstName = "CEO", LastName = "TEST", Email = "test@CustomerName-software.com",
            });

        _identityContractMock
            .Setup(x => x.HasUserAnyRoleAsync(
                request.ApproverId,
                It.IsAny<List<RoleType>>(),
                _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<ActionNotAllowedException>()
            .WithMessage(ExceptionConstants.ApproverDoesNotHavePermission);
    }

    [Fact]
    public async Task UpdateEquipmentAsync_WhenEquipmentTypeWasNotFound_ThrowEntityNotFoundException()
    {
        // Arrange
        const int approverId = 1;

        var request = new UpdateEquipmentCommand(
            2,
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

        _getEquipmentApproverDbQueryMock
            .Setup(x => x.ExecuteAsync(approverId, _cancellationToken))
            .ReturnsAsync(new EquipmentUser
            {
                UserId = approverId,
                FirstName = "CEO", LastName = "TEST", Email = "test@CustomerName-software.com",
            });

        _identityContractMock
            .Setup(x => x.HasUserAnyRoleAsync(
                request.ApproverId,
                It.IsAny<List<RoleType>>(),
                _cancellationToken))
            .ReturnsAsync(true);

        _isEquipmentTypeExistDbQueryMock
            .Setup(x => x.IsEquipmentTypeExist(request.TypeId, _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should()
            .ThrowAsync<EntityNotFoundException>()
            .WithMessage(ExceptionConstants.EquipmentTypeNotFound);
    }

    [Fact]
    public async Task UpdateEquipmentAsync_WhenEquipmentWasNotFound_ThrowEntityNotFoundException()
    {
        // Arrange
        const int approverId = 1;

        var request = new UpdateEquipmentCommand(
            2,
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

        _getEquipmentApproverDbQueryMock
            .Setup(x => x.ExecuteAsync(approverId, _cancellationToken))
            .ReturnsAsync(new EquipmentUser
            {
                UserId = approverId,
                FirstName = "CEO",
                LastName = "TEST",
                Email = "test@CustomerName-software.com",
            });

        _identityContractMock
            .Setup(x => x.HasUserAnyRoleAsync(
                request.ApproverId,
                It.IsAny<List<RoleType>>(),
                _cancellationToken))
            .ReturnsAsync(true);

        _isEquipmentTypeExistDbQueryMock
            .Setup(x => x.IsEquipmentTypeExist(request.TypeId, _cancellationToken))
            .ReturnsAsync(true);

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(request.Id, _cancellationToken))
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
    public async Task UpdateEquipmentAsync_WhenUpdateIsSuccessful_ShouldReturnUpdatedEquipmentDto()
    {
        // Arrange
        const int approverId = 1;
        const int equipmentId = 2;
        const string equipmentTypeId = "Headset";

        var request = new UpdateEquipmentCommand(
            equipmentId,
            "Name",
            equipmentTypeId,
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

        var expectedUpdatedEquipmentDto = new UpdatedEquipmentDto
        (
            Id: equipmentId,
            Name: request.Name,
            TypeId: request.TypeId,
            Location: request.Location,
            SerialNumber: request.SerialNumber,
            PurchasePrice: request.PurchasePrice,
            PurchaseCurrency: request.PurchaseCurrency,
            PurchasePriceUsd: request.PurchasePriceUsd,
            PurchaseDate: request.PurchaseDate,
            PurchasePlace: request.PurchasePlace,
            GuaranteeDate: request.GuaranteeDate,
            Characteristics: request.Characteristics,
            Comment: request.Comment,
            ApproverId: approverId,
            InvoiceNumber: request.InvoiceNumber
        );

        _getEquipmentApproverDbQueryMock
            .Setup(x => x.ExecuteAsync(approverId, _cancellationToken))
            .ReturnsAsync(new EquipmentUser
            {
                UserId = approverId,
                FirstName = "CEO",
                LastName = "TEST",
                Email = "test@CustomerName-software.com",
            });

        _identityContractMock
            .Setup(x => x.HasUserAnyRoleAsync(
                request.ApproverId,
                It.IsAny<List<RoleType>>(),
                _cancellationToken))
            .ReturnsAsync(true);

        _isEquipmentTypeExistDbQueryMock
            .Setup(x => x.IsEquipmentTypeExist(request.TypeId, _cancellationToken))
            .ReturnsAsync(true);

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(request.Id, _cancellationToken))
            .ReturnsAsync(true);

        _isEquipmentExistByIdDbQueryMock
            .Setup(x => x.ExecuteAsync(request.Id, _cancellationToken))
            .ReturnsAsync(true);

        _getEquipmentTypeByIdDbQueryMock
            .Setup(x => x.GetEquipmentTypeByIdAsync(request.TypeId, _cancellationToken))
            .ReturnsAsync(new EquipmentType
            (
                equipmentTypeId,
                "Headset",
                "Headset"
            ));

        _updateEquipmentDbQueryMock
            .Setup(x => x.ExecuteAsync(It.IsAny<UpdateEquipmentDbQueryRequest>(), _cancellationToken))
            .ReturnsAsync(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedUpdatedEquipmentDto);
    }

}

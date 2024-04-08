using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentById;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Queries;

public class GetEquipmentByIdQueryTests
{
    private readonly Mock<IEquipmentProvider> _equipmentProviderMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IClockService> _clockServiceMock = new();
    private readonly Mock<IAuthenticatedUserContext> _authenticatedUserContextMock = new();

    private readonly CancellationToken _cancellationToken;
    private readonly GetEquipmentByIdQueryHandler _handler;

    public GetEquipmentByIdQueryTests()
    {
        _cancellationToken = CancellationToken.None;
        _handler = new GetEquipmentByIdQueryHandler(
            _equipmentProviderMock.Object,
            _mapperMock.Object,
            _clockServiceMock.Object,
            _authenticatedUserContextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenEquipmentDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var equipmentId = 1;
        var request = new GetEquipmentByIdQuery(equipmentId);

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync((Domain.Equipment?)null);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Theory]
    [MemberData(nameof(GetDepartmentsTestData))]
    public async Task Handle_WhenUserIsHeadOfDepartmentAndEquipmentIsAssignedToUserOfOtherDepartment_ShouldThrowInvalidDataAppException(
        string userDepartmentId,
        string? holderDepartmentId)
    {
        // Arrange
        var equipmentId = 2;
        var request = new GetEquipmentByIdQuery(equipmentId);

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            Characteristics = "Characteristics",
            Name = "Name",
            PurchasePlace = "PurchasePlace",
            SerialNumber = "SerialNumber",
            Approver = new EquipmentUser
            {
                UserId = 22,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@CustomerName-software.com"
            },
            Assigns =
            [
                new EquipmentAssign
                {
                    AssignedToUserId = 5,
                    EquipmentId = request.Id,
                    IssueDate = DateTime.UtcNow.Date.AddDays(-5),
                    User = new EquipmentUser
                    {
                        UserId = 5,
                        FirstName = "Lorem",
                        LastName = "Ipsum",
                        Email = "lorem.ipsum@CustomerName-software.com",
                        DepartmentId = holderDepartmentId!
                    }
                }
            ]
        };

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(equipment);
        _authenticatedUserContextMock.Setup(x => x.Role)
            .Returns(RoleType.HeadOfDepartment);
        _authenticatedUserContextMock.Setup(x => x.DepartmentId)
            .Returns(userDepartmentId);

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<ActionNotAllowedException>();
    }

    [Fact]
    public async Task Handle_WhenUserIsHeadOfDepartmentAndEquipmentIsNotAssigned_ShouldThrowActionNotAllowedException()
    {
        // Arrange
        var equipmentId = 3;
        var request = new GetEquipmentByIdQuery(equipmentId);

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            Characteristics = "Characteristics",
            Name = "Name",
            PurchasePlace = "PurchasePlace",
            SerialNumber = "SerialNumber",
            Approver = new EquipmentUser
            {
                UserId = 22,
                FirstName = "Lorem",
                LastName = "Ipsum",
                Email = "lorem.ipsum@CustomerName-software.com"
            }
        };

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(equipment);
        _authenticatedUserContextMock.Setup(x => x.Role)
            .Returns(RoleType.HeadOfDepartment);
        _authenticatedUserContextMock.Setup(x => x.DepartmentId)
            .Returns("Net");

        // Act
        var result = () => _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        await result.Should().ThrowAsync<ActionNotAllowedException>();
    }

    [Fact]
    public async Task Handle_WhenEquipmentWithHolder_ShouldReturnDtoWithMappedHolder()
    {
        // Arrange
        var equipmentId = 4;
        var request = new GetEquipmentByIdQuery(equipmentId);

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            Characteristics = "Characteristics",
            Name = "Name",
            PurchasePlace = "PurchasePlace",
            SerialNumber = "SerialNumber",
            Approver = new EquipmentUser
            {
                UserId = 22,
                FirstName = "Lorem",
                LastName = "Ipsum",
                Email = "lorem.ipsum@CustomerName-software.com"
            },
            Assigns =
            [
                new EquipmentAssign
                {
                    AssignedToUserId = 5,
                    EquipmentId = request.Id,
                    IssueDate = DateTime.UtcNow.Date.AddDays(-5),
                    User = new EquipmentUser
                    {
                        UserId = 5,
                        FirstName = "Lorem",
                        LastName = "Ipsum",
                        Email = "lorem.ipsum@CustomerName-software.com"
                    }
                }
            ]
        };

        var activeHolderDto = new EquipmentHolderDto
        {
            Id = equipment.Assigns[0].User!.UserId,
            FirstName = equipment.Assigns[0].User!.FirstName,
            LastName = equipment.Assigns[0].User!.LastName,
            IssueDate = equipment.Assigns[0].IssueDate
        };

        var equipmentDto = new EquipmentDto
        {
            Id = equipment.Id,
            Characteristics = equipment.Characteristics,
            Location = equipment.Location,
            Name = equipment.Name,
            PurchasePlace = equipment.PurchasePlace,
            SerialNumber = equipment.SerialNumber,
            Approver = new ApproverDto
            {
                Id = equipment.Approver.UserId,
                FirstName = equipment.Approver.FirstName,
                LastName = equipment.Approver.LastName
            }
        };

        var expectedEquipmentDto = new EquipmentDto
        {
            Id = equipment.Id,
            Characteristics = equipment.Characteristics,
            Location = equipment.Location,
            Name = equipment.Name,
            PurchasePlace = equipment.PurchasePlace,
            SerialNumber = equipment.SerialNumber,
            ActiveHolder = activeHolderDto,
            Approver = new ApproverDto
            {
                Id = equipment.Approver.UserId,
                FirstName = equipment.Approver.FirstName,
                LastName = equipment.Approver.LastName
            }
        };

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(equipment);
        _mapperMock.Setup(x => x.Map<EquipmentHolderDto>(It.IsAny<EquipmentAssign>()))
            .Returns(activeHolderDto);
        _mapperMock.Setup(x => x.Map<EquipmentDto>(equipment))
            .Returns(equipmentDto);
        _authenticatedUserContextMock.Setup(x => x.Role)
            .Returns(RoleType.CEO);
        _clockServiceMock.Setup(x => x.UtcNow)
            .Returns(DateTime.UtcNow);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(
            expectedEquipmentDto,
            options => options.Excluding(x => x.ActiveHolder));
        result.ActiveHolder.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WhenEquipmentWithoutHolder_ShouldReturnDtoWithoutHolder()
    {
        // Arrange
        var equipmentId = 5;
        var request = new GetEquipmentByIdQuery(equipmentId);

        var equipment = new Domain.Equipment
        {
            Id = equipmentId,
            Characteristics = "Characteristics",
            Name = "Name",
            PurchasePlace = "PurchasePlace",
            SerialNumber = "SerialNumber",
            Approver = new EquipmentUser
            {
                UserId = 22,
                FirstName = "Lorem",
                LastName = "Ipsum",
                Email = "lorem.ipsum@CustomerName-software.com"
            }
        };

        var equipmentDto = new EquipmentDto
        {
            Id = equipment.Id,
            Characteristics = equipment.Characteristics,
            Location = equipment.Location,
            Name = equipment.Name,
            PurchasePlace = equipment.PurchasePlace,
            SerialNumber = equipment.SerialNumber,
            Approver = new ApproverDto
            {
                Id = equipment.Approver.UserId,
                FirstName = equipment.Approver.FirstName,
                LastName = equipment.Approver.LastName
            }
        };

        var expectedEquipmentDto = new EquipmentDto
        {
            Id = equipment.Id,
            Characteristics = equipment.Characteristics,
            Location = equipment.Location,
            Name = equipment.Name,
            PurchasePlace = equipment.PurchasePlace,
            SerialNumber = equipment.SerialNumber,
            Approver = new ApproverDto
            {
                Id = equipment.Approver.UserId,
                FirstName = equipment.Approver.FirstName,
                LastName = equipment.Approver.LastName
            }
        };

        _equipmentProviderMock.Setup(x => x.GetEquipmentByIdAsync(
                equipmentId,
                _cancellationToken))
            .ReturnsAsync(equipment);
        _mapperMock.Setup(x => x.Map<EquipmentDto>(equipment))
            .Returns(equipmentDto);
        _authenticatedUserContextMock.Setup(x => x.Role)
            .Returns(RoleType.CTO);

        // Act
        var result = await _handler.Handle(
            request,
            _cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(expectedEquipmentDto);
    }

    public static IEnumerable<object?[]> GetDepartmentsTestData()
    {
        yield return ["Java", "DevOps"];
        yield return ["Net", "Js"];
        yield return ["Js", null];
    }
}

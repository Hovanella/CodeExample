using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetEquipmentUsersLookup;
using CustomerName.Portal.Equipment.Domain;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentUsersLookup;
using CustomerName.Portal.Framework.Utils.Extensions;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Queries;

public class GetEquipmentUsersLookupQueryHandlerTests
{
    private readonly Mock<IGetEquipmentUsersLookupDbQuery> _equipmentUsersLookupDbQueryMock = new();

    private readonly GetEquipmentUsersLookupQueryHandler _handler;
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    public GetEquipmentUsersLookupQueryHandlerTests()
    {
        _handler = new GetEquipmentUsersLookupQueryHandler(_equipmentUsersLookupDbQueryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnActiveEquipmentUsers()
    {
        //Arrange
        var expectedUsers = new List<EquipmentUserLookup>()
        {
            new(1, Guid.NewGuid().ToAlphabetic()), new(2, Guid.NewGuid().ToAlphabetic())
        };

        _equipmentUsersLookupDbQueryMock
            .Setup(x => x.ExecuteAsync(_cancellationToken))
            .ReturnsAsync(expectedUsers);

        //Act
        var result = await _handler.Handle(new GetEquipmentUsersLookupQuery(),_cancellationToken);

        //Assert
        _equipmentUsersLookupDbQueryMock
            .Verify(x=>x.ExecuteAsync(_cancellationToken),Times.Once);

        result.Should().BeEquivalentTo(expectedUsers);
    }
}

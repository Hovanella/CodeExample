using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPageableEquipmentsWithFilterOptions;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentActiveHolder;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.GetPageableEquipments.GetPossibleEquipmentApprovers;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Equipment.UseCases.BusinessRules;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipments;
using CustomerName.Portal.Framework.Context;
using CustomerName.Portal.Framework.OData;
using CustomerName.Portal.Framework.Utils;
using CustomerName.Portal.Identity.Contract;

namespace CustomerName.Portal.Equipment.UnitTests.UseCases.EquipmentTests.Queries;

public class GetEquipmentsQueryHandlerTests
{
    private readonly Mock<IAuthenticatedUserContext> _authedUserMock = new();
    private readonly Mock<IIsAvailableEquipmentsWithoutAssignersBusinessRule> _isAvailableEquipmentsWithoutAssignersMock = new();
    private readonly Mock<IIdentityContract> _identityContractMock = new();
    private readonly Mock<IGetPageableEquipmentsWithFilterOptionsDbQuery> _getPageableEquipmentsDbQueryMock = new();
    private readonly Mock<IGetPossibleEquipmentApproversDbQuery> _getPossibleEquipmentApproversDbQueryMock = new();
    private readonly Mock<IGetPossibleEquipmentActiveHoldersDbQuery> _getPossibleEquipmentActiveHoldersDbQuery = new ();

    private readonly CancellationToken _cancellationToken = new();
    private readonly GetEquipmentsQueryHandler _handler;

    public GetEquipmentsQueryHandlerTests()
    {
        _handler = new GetEquipmentsQueryHandler(
            _authedUserMock.Object,
            _isAvailableEquipmentsWithoutAssignersMock.Object,
            _identityContractMock.Object,
            _getPageableEquipmentsDbQueryMock.Object,
            _getPossibleEquipmentApproversDbQueryMock.Object,
            _getPossibleEquipmentActiveHoldersDbQuery.Object
            );
    }

    [Fact]
    public async Task GetEquipments_ShouldCallEquipmentDbQueries()
    {
        // Arrange
        var pageableEquipments = PageableListOfItems<OdpEquipment>
            .FromItems([])
            .WithPaging(1, 10)
            .WithTotal(0);

        var getPageableEquipmentsDbQueryResponse = new GetPageableEquipmentsWithFilterOptionsDbQueryResponse(pageableEquipments, false, [], []);

        var queryOptions = new Dictionary<string, string?>
        {
            ["top"] = "10",
            ["skip"] = "0"
        };

        var oDataQueryOptions = ODataQueryOptionsMock.BuildMock<OdpEquipment>(queryOptions);
        var departmentId = (string?)null;
        var isAvailableEquipmentsWithoutAssigners = true;

        _isAvailableEquipmentsWithoutAssignersMock.Setup(x =>
            x.IsCorrespondsToBusiness(_authedUserMock.Object, _cancellationToken))
            .ReturnsAsync(isAvailableEquipmentsWithoutAssigners);

        _getPageableEquipmentsDbQueryMock
            .Setup(x => x.ExecuteAsync(
                new GetPageableEquipmentsDbQueryRequest( oDataQueryOptions,
                    departmentId,
                    isAvailableEquipmentsWithoutAssigners),
                _cancellationToken
             ))
            .ReturnsAsync(getPageableEquipmentsDbQueryResponse);

        _identityContractMock
            .Setup(x => x.GetManagersIds(_cancellationToken))
            .ReturnsAsync([]);

        _getPossibleEquipmentActiveHoldersDbQuery
            .Setup(x => x.ExecuteAsync(
                new GetPossibleEquipmentActiveHoldersDbQueryRequest(departmentId,false),
                _cancellationToken))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(
            new GetEquipmentsQuery(oDataQueryOptions),
            _cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.Page.Should().Be(pageableEquipments.Page);
        result.PageSize.Should().Be(pageableEquipments.PageSize);
        result.Total.Should().Be(pageableEquipments.Total);

        _isAvailableEquipmentsWithoutAssignersMock
            .Verify(x => x.IsCorrespondsToBusiness(_authedUserMock.Object, _cancellationToken), Times.Once);

        _getPageableEquipmentsDbQueryMock
            .Verify(x=>x.ExecuteAsync(It.IsAny<GetPageableEquipmentsDbQueryRequest>(),_cancellationToken));

        _identityContractMock
            .Verify(x => x.GetManagersIds(_cancellationToken));

    }

}

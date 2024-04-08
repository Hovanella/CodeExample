using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByUserId;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentUsersLookup;
using CustomerName.Portal.Framework.Api.Attributes;
using CustomerName.Portal.Framework.Api.Controllers;
using CustomerName.Portal.Framework.Api.Response;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.Controllers.Controllers;

[Area("[Equipment] Users")]
public class UserEquipmentsController : BaseApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UserEquipmentsController(
        ISender mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Get users by equipment
    /// Allowed Roles : Auth
    /// </summary>
    /// <param name="userId">ID of user</param>
    /// <returns>user equipment model</returns>
    [ProducesResponseType(typeof(List<UserEquipmentOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpGet("/api/users/{userId:int}/equipment")]
    [AuthorizeRoles(RoleType.SystemAdministrator, RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment,RoleType.Employee)]
    public async Task<ActionResult<List<UserEquipmentOutput>>> GetUsersEquipmentsAsync([FromRoute] int userId)
    {
        var result = await _mediator.Send(
            new GetEquipmentByUserIdQuery(userId),
            HttpContext.RequestAborted);

        return _mapper.Map<List<UserEquipmentOutput>>(result);
    }

    [HttpGet("/api/equipment-users/lookup")]
    [AuthorizeRoles(RoleType.SystemAdministrator, RoleType.CEO, RoleType.CTO)]
    public async Task<ActionResult<List<EquipmentUserLookup>>> GetUsersLookupAsync()
    {
        var result = await _mediator.Send(
            new GetEquipmentUsersLookupQuery(),
            HttpContext.RequestAborted);

        return result
            .Select(x => new EquipmentUserLookup(x.Id, x.FullName))
            .ToList();
    }
}

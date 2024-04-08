using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerName.Portal.Equipment.Controllers.Contracts.Input;
using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.Controllers.Mappers;
using CustomerName.Portal.Equipment.UseCases.Commands.AssignUserToEquipment;
using CustomerName.Portal.Equipment.UseCases.Commands.DeleteEquipmentHistoryRecord;
using CustomerName.Portal.Equipment.UseCases.Commands.PopulateReturnDate;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentHistory;
using CustomerName.Portal.Framework.Api.Attributes;
using CustomerName.Portal.Framework.Api.Controllers;
using CustomerName.Portal.Framework.Api.Response;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.Controllers.Controllers;

[Area("[Equipment] History")]
[Route("api/[controller]/{equipmentId:int}")]
internal class EquipmentHistoryController : BaseApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    private readonly IActiveHolderDtoToEquipmentHolderOutputMapper _activeHolderDtoToEquipmentHolderOutputMapper;

    public EquipmentHistoryController(
        ISender sender,
        IMapper mapper,
        IActiveHolderDtoToEquipmentHolderOutputMapper activeHolderDtoToEquipmentHolderOutputMapper)
    {
        _sender = sender;
        _mapper = mapper;
        _activeHolderDtoToEquipmentHolderOutputMapper = activeHolderDtoToEquipmentHolderOutputMapper;
    }

    /// <summary>
    /// Get equipment holders by equipment id
    /// Allowed Roles : SystemAdministrator, CEO, CTO
    /// </summary>
    /// <param name="equipmentId">Id of equipment</param>
    /// <returns>Holders of equipment</returns>
    [ProducesResponseType(typeof(EquipmentHolderOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpGet("history")]
    [AuthorizeRoles(RoleType.CTO, RoleType.CEO, RoleType.SystemAdministrator)]
    public async Task<ActionResult<List<EquipmentHolderOutput>>> GetEquipmentHistoryAsync(int equipmentId)
    {
        var result = await _sender.Send(
            new GetEquipmentHistoryQuery(equipmentId),
            HttpContext.RequestAborted);

        return Ok(_mapper.Map<List<EquipmentHolderOutput>>(result));
    }

    /// <summary>
    /// assign a user to equipment
    /// Allowed Roles : SystemAdministrator, CEO, CTO
    /// </summary>
    /// <param name="equipmentId">Id of equipment</param>
    /// <param name="model">Model which storage issue date and id of holder</param>
    /// <returns>Holder model</returns>
    [ProducesResponseType(typeof(EquipmentHolderOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpPost("assign")]
    [AuthorizeRoles(RoleType.CEO, RoleType.CTO, RoleType.SystemAdministrator)]
    public async Task<ActionResult<EquipmentHolderOutput>> AssignUserToEquipmentAsync(
        [FromRoute] int equipmentId,
        [FromBody] AssignEquipmentInput model)
    {
        var assignment = await _sender.Send(
            new AssignUserToEquipmentCommand(
                equipmentId,
                model.UserId,
                model.IssueDate),
            HttpContext.RequestAborted);

        return Ok(_activeHolderDtoToEquipmentHolderOutputMapper.Map(assignment));
    }

    /// <summary>
    /// UnAssign a user from equipment
    /// Allowed Roles : SystemAdministrator, CEO, CTO
    /// </summary>
    /// <param name="equipmentId">Id of equipment</param>
    /// <param name="model">object with return date</param>
    /// <returns>Holder model</returns>
    [ProducesResponseType(typeof(EquipmentHolderOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpPost("return")]
    [AuthorizeRoles(RoleType.CEO, RoleType.CTO, RoleType.SystemAdministrator)]
    public async Task<ActionResult<EquipmentHolderOutput>> UnAssignUserFromEquipmentAsync(
        [FromRoute] int equipmentId,
        [FromBody] ReturnEquipmentInput model)
    {
        var result = await _sender.Send(
            new SetReturnDateToAssignCommand(
                equipmentId,
                model.ReturnDate),
            HttpContext.RequestAborted);

        return Ok(_activeHolderDtoToEquipmentHolderOutputMapper.Map(result));
    }

    /// <summary>
    /// remove history without returnDate about equipment
    /// Allowed Roles : SystemAdministrator, CEO, CTO
    /// </summary>
    /// <param name="equipmentId">Id of equipment</param>
    /// <returns>Holder model</returns>
    [ProducesResponseType(typeof(EquipmentHolderOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpDelete("history")]
    [AuthorizeRoles(RoleType.CEO, RoleType.CTO, RoleType.SystemAdministrator)]
    public async Task<ActionResult> RemoveHistoryEquipmentAsync([FromRoute] int equipmentId)
    {
        var deletedAssignment = await _sender.Send(
            new DeleteEquipmentHistoryCommand(equipmentId),
            HttpContext.RequestAborted);

        return Ok(_mapper.Map<EquipmentHolderOutput>(deletedAssignment));
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerName.Portal.Authentication;
using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.Controllers.Mappers.EquipmentReports;
using CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipmentReport;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportById;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportRelevancePeriods;
using CustomerName.Portal.Framework.Api.Attributes;
using CustomerName.Portal.Framework.Api.Controllers;
using CustomerName.Portal.Framework.Api.Response;
using CustomerName.Portal.Framework.Core;

namespace CustomerName.Portal.Equipment.Controllers.Controllers;

[Area("[Equipment] Reports")]
internal class EquipmentReportsController : BaseApiController
{
    private readonly IEquipmentReportDtoToEquipmentReportOutputMapper _reportDtoToEquipmentReportOutputMapper;
    private readonly IRelevancePeriodDtoToRelevancePeriodOutputMapper _relevancePeriodDtoToRelevancePeriodOutputMapper;
    private readonly ISender _sender;

    public EquipmentReportsController(
        IEquipmentReportDtoToEquipmentReportOutputMapper reportDtoToEquipmentReportOutputMapper,
        IRelevancePeriodDtoToRelevancePeriodOutputMapper relevancePeriodDtoToRelevancePeriodOutputMapper,
        ISender sender)
    {
        _reportDtoToEquipmentReportOutputMapper = reportDtoToEquipmentReportOutputMapper;
        _relevancePeriodDtoToRelevancePeriodOutputMapper = relevancePeriodDtoToRelevancePeriodOutputMapper;
        _sender = sender;
    }

    /// <summary>
    /// Upload file with an equipment report json data to server
    /// Allowed Roles: Anonymous
    /// </summary>
    /// <param name="file">File form to upload</param>
    /// <returns>Equipment report</returns>
    [ProducesResponseType(typeof(EquipmentReportOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AllowAnonymous]
    [ServiceFilter(typeof(SecurityHashHeaderAttribute))]
    public async Task<ActionResult<EquipmentReportOutput>> CreateEquipmentReportAsync(
        IFormFile file)
    {
        var result = await _sender.Send(
            new CreateEquipmentReportCommand(file),
            HttpContext.RequestAborted);

        return CreatedAtAction(
            nameof(GetEquipmentReportAsync),
            new { result.Id },
            _reportDtoToEquipmentReportOutputMapper.Map(result));
    }

    /// <summary>
    /// Get equipment report relevance periods by serial number
    /// Allowed Roles : SystemAdministrator, CEO, CTO, HeadOfDepartment
    /// </summary>
    /// <param name="serialNumber">Equipment serial number</param>
    /// <returns>Equipment report relevance periods</returns>
    [ProducesResponseType(typeof(EquipmentReportOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("{serialNumber}")]
    [AuthorizeRoles(RoleType.SystemAdministrator, RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment)]
    public async Task<ActionResult<List<EquipmentReportRelevancePeriodOutput>>> GetEquipmentReportRelevancePeriodsAsync(
        [FromRoute] string serialNumber)
    {
        var result = await _sender.Send(
            new GetEquipmentReportRelevancePeriodsQuery(serialNumber),
            HttpContext.RequestAborted);

        return Ok(_relevancePeriodDtoToRelevancePeriodOutputMapper.Map(result).ToList());
    }

    /// <summary>
    /// Get equipment report by id
    /// Allowed Roles : SystemAdministrator, CEO, CTO, HeadOfDepartment
    /// </summary>
    /// <param name="id">Equipment Report id</param>
    /// <returns>Equipment Report</returns>
    [ProducesResponseType(typeof(EquipmentReportOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("{id:int}")]
    [AuthorizeRoles(RoleType.SystemAdministrator, RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment)]
    public async Task<ActionResult<EquipmentReportOutput>> GetEquipmentReportAsync(
        [FromRoute] int id)
    {
        var result = await _sender.Send(
            new GetEquipmentReportByIdQuery(id),
            HttpContext.RequestAborted);

        return Ok(_reportDtoToEquipmentReportOutputMapper.Map(result));
    }
}

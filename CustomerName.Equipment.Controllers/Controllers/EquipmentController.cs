using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using CustomerName.Portal.Equipment.Controllers.Contracts.Input;
using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.Controllers.Mappers;
using CustomerName.Portal.Equipment.Controllers.Mappers.PageableEquipments;
using CustomerName.Portal.Equipment.Domain.OData;
using CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipment;
using CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentById;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipments;
using CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByFile;
using CustomerName.Portal.Framework.Api.Attributes;
using CustomerName.Portal.Framework.Api.Controllers;
using CustomerName.Portal.Framework.Api.Response;
using CustomerName.Portal.Framework.Core;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.Controllers.Controllers;

[Area("[Equipment] Equipment")]
internal class EquipmentController : BaseApiController
{
    private readonly ISender _sender;
    private readonly IClockService _clock;

    private readonly IUpdatedEquipmentDtoToUpdatedEquipmentOutputMapper _updatedEquipmentDtoToUpdatedEquipmentOutputMapper;

    private readonly IPageableEquipmentDtoToPageableEquipmentsOutputMapper _pageableEquipmentDtoToPageableEquipmentsOutputMapper;
    private readonly IEquipmentDtoToEquipmentOutputMapper _equipmentDtoToEquipmentOutputMapper;

    public EquipmentController(
        ISender sender,
        IPageableEquipmentDtoToPageableEquipmentsOutputMapper pageableEquipmentDtoToPageableEquipmentsOutputMapper,
        IClockService clock,
        IEquipmentDtoToEquipmentOutputMapper equipmentDtoToEquipmentOutputMapper,
        IUpdatedEquipmentDtoToUpdatedEquipmentOutputMapper updatedEquipmentDtoToUpdatedEquipmentOutputMapper)
    {
        _sender = sender;
        _clock = clock;
        _updatedEquipmentDtoToUpdatedEquipmentOutputMapper = updatedEquipmentDtoToUpdatedEquipmentOutputMapper;
        _pageableEquipmentDtoToPageableEquipmentsOutputMapper = pageableEquipmentDtoToPageableEquipmentsOutputMapper;
        _equipmentDtoToEquipmentOutputMapper = equipmentDtoToEquipmentOutputMapper;
    }

    /// <summary>
    /// Get a list equipment
    /// Allowed Roles : SystemAdministrator, CEO, CTO, HeadOfDepartment
    /// </summary>
    /// <param name="queryOptions">parameters for searching, ordering and paging</param>
    /// <returns>A list of equipment</returns>
    [ProducesResponseType(typeof(PageableEquipmentsOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpGet]
    [AuthorizeRoles(RoleType.SystemAdministrator, RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment)]
    public async Task<ActionResult<PageableEquipmentsOutput>> GetPageableEquipmentsAsync(
        ODataQueryOptions<OdpEquipment> queryOptions)
    {
        var pageableEquipments = await _sender.Send(
            new GetEquipmentsQuery(queryOptions),
            HttpContext.RequestAborted);

        var outputModel = _pageableEquipmentDtoToPageableEquipmentsOutputMapper.Map(pageableEquipments);

        return Ok(outputModel);
    }

    /// <summary>
    /// Save a list of equipment to Csv or Excel file
    /// Allowed Roles : SystemAdministrator, CEO, CTO, HeadOfDepartment
    /// </summary>
    /// <param name="queryOptions">parameters for searching, ordering, paging and choosing format of file</param>
    /// <param name="format">parameter for choosing a type of file</param>
    /// <returns>void</returns>
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpGet("file/{format}")]
    [AuthorizeRoles(RoleType.SystemAdministrator, RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment)]
    public async Task<ActionResult> GetEquipmentsByFileAsync(ODataQueryOptions<OdpEquipment> queryOptions, DocumentType format)
    {
        var fileContents = await _sender.Send(new GetEquipmentsByFileQuery(queryOptions, format),
            HttpContext.RequestAborted);

        return format switch
        {
            DocumentType.Csv => File(
                fileContents,
                PortalConstants.Csv.ContentType,
                $"{PortalConstants.Csv.EquipmentsFileName}_{_clock.UtcNow.Date.ToString(PortalConstants.Equipment.FileExportDateFormat,CultureInfo.CurrentCulture)}{PortalConstants.Csv.EquipmentsFileExtension}"),
            DocumentType.Excel => File(
                fileContents,
                PortalConstants.Excel.ContentType,
                $"{PortalConstants.Excel.EquipmentsFileName}_{_clock.UtcNow.Date.ToString(PortalConstants.Equipment.FileExportDateFormat,CultureInfo.CurrentCulture)}{PortalConstants.Excel.EquipmentsFileExtension}"),
            _ => throw new ApplicationException("Unsupported file format")
        };
    }

    /// <summary>
    /// Get equipment by id
    /// Allowed Roles : SystemAdministrator, CEO, CTO, HeadOfDepartment
    /// </summary>
    /// <param name="id">id of the equipment</param>
    /// <returns>Equipment model</returns>
    [ProducesResponseType(typeof(EquipmentOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpGet("{id:int}", Name = nameof(GetEquipmentAsync))]
    [AuthorizeRoles(RoleType.SystemAdministrator, RoleType.CEO, RoleType.CTO, RoleType.HeadOfDepartment)]
    public async Task<ActionResult<EquipmentOutput>> GetEquipmentAsync(int id)
    {
        var result = await _sender.Send(
            new GetEquipmentByIdQuery(id),
            HttpContext.RequestAborted);

        return Ok(_equipmentDtoToEquipmentOutputMapper.Map(result));
    }

    /// <summary>
    /// Create new equipment
    /// Allowed Roles : SystemAdministrator, CEO, CTO
    /// </summary>
    /// <param name="model">model of a new equipment</param>
    /// <returns>Created equipment model</returns>
    [ProducesResponseType(typeof(EquipmentOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [AuthorizeRoles(RoleType.CEO, RoleType.CTO, RoleType.SystemAdministrator)]
    public async Task<ActionResult<EquipmentOutput>> CreateEquipmentAsync([FromBody] CreateEquipmentInput model)
    {
        var request = new CreateEquipmentCommand(
            model.Name,
            model.EquipmentTypeId,
            model.Location,
            model.SerialNumber,
            model.PurchasePrice,
            model.PurchaseCurrency,
            model.PurchasePriceUsd,
            model.PurchaseDate,
            model.PurchasePlace,
            model.GuaranteeDate,
            model.Characteristics,
            model.Comment,
            model.InvoiceNumber,
            model.ApproverId);

        var result = await _sender.Send(
            request,
            HttpContext.RequestAborted);

        return CreatedAtAction(
            nameof(GetEquipmentAsync),
            new { result.Id },
            _equipmentDtoToEquipmentOutputMapper.Map(result));
    }

    /// <summary>
    /// Update equipment
    /// Allowed Roles : SystemAdministrator, CEO, CTO
    /// </summary>
    /// <param name="model">model of updating equipment</param>
    /// <returns>Updated equipment model</returns>
    [ProducesResponseType(typeof(EquipmentOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionResponse),StatusCodes.Status500InternalServerError)]
    [HttpPut]
    [AuthorizeRoles(RoleType.CEO, RoleType.CTO, RoleType.SystemAdministrator)]
    public async Task<ActionResult<UpdatedEquipmentOutput>> UpdateEquipmentAsync([FromBody] UpdateEquipmentInput model)
    {
        var request = new UpdateEquipmentCommand(
            model.Id,
            model.Name,
            model.EquipmentTypeId,
            model.Location,
            model.SerialNumber,
            model.PurchasePrice,
            model.PurchaseCurrency,
            model.PurchasePriceUsd,
            model.PurchaseDate,
            model.PurchasePlace,
            model.GuaranteeDate,
            model.Characteristics,
            model.Comment,
            model.InvoiceNumber,
            model.ApproverId);

        var equipmentDto = await _sender.Send(
            request,
            HttpContext.RequestAborted);

        return Ok(_updatedEquipmentDtoToUpdatedEquipmentOutputMapper.Map(equipmentDto));
    }
}

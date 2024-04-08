using System.Text.Json;
using System.Text.Json.Nodes;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.DbQueries.CreateEquipmentReport;
using CustomerName.Portal.Equipment.UseCases.Constants;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Equipment.UseCases.Mappers.EquipmentReports;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Framework.UseCases.Abstractions.Services;

namespace CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipmentReport;

internal class CreateEquipmentReportCommandHandler :
    ICommandHandler<CreateEquipmentReportCommand, EquipmentReportDto>
{
    private const string s_assemblyDatePropertyName = "CreationDate";
    private const string s_serialNumberPropertyName = "HardwareSerialNumber";
    private const string s_appTracingInfo = "AppTracingInfo";

    private readonly ICreateEquipmentReportDbQuery _createEquipmentReportDbQuery;
    private readonly IGetEquipmentReportBySerialNumberAndDataHashDbQuery _getEquipmentReportDbQuery;
    private readonly IUpdateEquipmentReportRelevancePeriodDbQuery _updateEquipmentReportPeriodDbQuery;
    private readonly IIsEquipmentExistingBySerialNumberDbQuery _isEquipmentExistingBySerialNumberDbQuery;

    private readonly IHashService _hashService;

    private readonly IEquipmentReportToEquipmentReportDtoMapper _mapper;

    public CreateEquipmentReportCommandHandler(
        ICreateEquipmentReportDbQuery createEquipmentReportDbQuery,
        IGetEquipmentReportBySerialNumberAndDataHashDbQuery getEquipmentReportDbQuery,
        IUpdateEquipmentReportRelevancePeriodDbQuery updateEquipmentReportPeriodDbQuery,
        IIsEquipmentExistingBySerialNumberDbQuery isEquipmentExistingBySerialNumberDbQuery,
        IEquipmentReportToEquipmentReportDtoMapper mapper,
        IHashService hashService)
    {
        _createEquipmentReportDbQuery = createEquipmentReportDbQuery;
        _getEquipmentReportDbQuery = getEquipmentReportDbQuery;
        _updateEquipmentReportPeriodDbQuery = updateEquipmentReportPeriodDbQuery;
        _isEquipmentExistingBySerialNumberDbQuery = isEquipmentExistingBySerialNumberDbQuery;
        _hashService = hashService;
        _mapper = mapper;
    }

    public async Task<EquipmentReportDto> Handle(
        CreateEquipmentReportCommand request,
        CancellationToken cancellationToken)
    {
        await using var stream = request.File.OpenReadStream();

        var jsonDocument = await JsonDocument.ParseAsync(stream,
            cancellationToken: cancellationToken);

        var jsonRootElement = jsonDocument.RootElement;

        var jsonObject = JsonObject.Create(jsonRootElement);

        if (jsonObject is null)
        {
            throw new InvalidDataAppException();
        }

        var serialNumber = await GetSerialNumberFromJsonObjectAsync(
                                    jsonObject,
                                    cancellationToken);

        var creationDate = GetCreationDateTokenFromJsonObject(jsonObject);

        var jsonData = jsonObject.ToJsonString(
            new JsonSerializerOptions { WriteIndented = false });

        var jsonDataHash = GetPreparedJsonDataHash(jsonObject);

        var equipmentReport = await _getEquipmentReportDbQuery
            .GetEquipmentReportBySerialNumberAndDataHashAsync(
                serialNumber,
                jsonDataHash,
                cancellationToken);

        if (equipmentReport is null)
        {
            var utcCreationDateTime = creationDate.ToUniversalTime();

            var dbQueryRequest = new CreateEquipmentReportDbQueryRequest(
                serialNumber,
                jsonData,
                jsonDataHash,
                utcCreationDateTime);

            equipmentReport = await _createEquipmentReportDbQuery
                                    .CreateEquipmentReportAsync(
                                        dbQueryRequest,
                                        cancellationToken);
        }
        else
        {
            await _updateEquipmentReportPeriodDbQuery
                  .UpdateEquipmentReportRelevancePeriodToUtcDateAsync(
                      equipmentReport.SerialNumber,
                      equipmentReport.Id,
                      cancellationToken);
        }

        var createdEquipmentReportDto = _mapper.Map(equipmentReport);

        return createdEquipmentReportDto;
    }

    private async Task<string> GetSerialNumberFromJsonObjectAsync(
        JsonObject jsonObject,
        CancellationToken cancellationToken)
    {
        jsonObject.TryGetPropertyValue(s_serialNumberPropertyName, out var serialNumberProperty);

        var serialNumber = serialNumberProperty?.GetValue<string>();
        jsonObject.Remove(s_serialNumberPropertyName);

        if (serialNumber is null)
        {
            throw new InvalidDataAppException(
                ExceptionConstants.EquipmentReportSerialNumberNotFound);
        }

        var isEquipmentExisting = await _isEquipmentExistingBySerialNumberDbQuery
                                        .IsEquipmentExistingBySerialNumberAsync(
                                            serialNumber,
                                            cancellationToken);

        if (!isEquipmentExisting)
        {
            throw new InvalidDataAppException(
                ExceptionConstants.EquipmentNotFound);
        }

        return serialNumber.ToLower();
    }

    private DateTime GetCreationDateTokenFromJsonObject(
        JsonObject jsonObject)
    {
        jsonObject.TryGetPropertyValue(s_assemblyDatePropertyName, out var creationDateProperty);

        var creationDate = creationDateProperty?.GetValue<DateTime>();

        if (creationDate is null)
        {
            throw new InvalidDataAppException(
                ExceptionConstants.EquipmentReportAssemblyDateNotFound);
        }

        jsonObject.Remove(s_assemblyDatePropertyName);

        return (DateTime)creationDate;
    }

    private string GetPreparedJsonDataHash(
        JsonObject jsonObject)
    {
        jsonObject.Remove(s_appTracingInfo);

        return _hashService.GetSha256Hash(jsonObject.ToJsonString(
                            new JsonSerializerOptions { WriteIndented = false }));
    }
}

using Microsoft.AspNetCore.Http;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;
using CustomerName.Portal.Framework.UseCases.Abstractions.Transactions;

namespace CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipmentReport;

internal record CreateEquipmentReportCommand(
    IFormFile File) : ICommand<EquipmentReportDto>, IEquipmentRequest, ITransactionalRequest;

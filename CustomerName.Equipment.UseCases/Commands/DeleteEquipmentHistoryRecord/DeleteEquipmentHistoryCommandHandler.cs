using AutoMapper;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Providers;
using CustomerName.Portal.Equipment.DataAccess.Abstractions.Repositories;
using CustomerName.Portal.Equipment.UseCases.Dto;
using CustomerName.Portal.Framework.Exceptions;
using CustomerName.Portal.Framework.UseCases.Abstractions.Commands;

namespace CustomerName.Portal.Equipment.UseCases.Commands.DeleteEquipmentHistoryRecord;

internal class DeleteEquipmentHistoryCommandHandler : ICommandHandler<DeleteEquipmentHistoryCommand, EquipmentHolderDto>
{
    private readonly IEquipmentProvider _equipmentProvider;
    private readonly IEquipmentAssignRepository _equipmentAssignRepository;
    private readonly IMapper _mapper;

    public DeleteEquipmentHistoryCommandHandler(
        IEquipmentProvider equipmentProvider,
        IEquipmentAssignRepository equipmentAssignRepository,
        IMapper mapper)
    {
        _equipmentProvider = equipmentProvider;
        _equipmentAssignRepository = equipmentAssignRepository;
        _mapper = mapper;
    }

    public async Task<EquipmentHolderDto> Handle(
        DeleteEquipmentHistoryCommand request,
        CancellationToken cancellationToken)
    {
        var equipment = await _equipmentProvider.GetEquipmentByIdAsync(
            request.EquipmentId,
            cancellationToken)
                ?? throw new EntityNotFoundException($"Equipment with id = \"{request.EquipmentId}\" does not exist");

        if (equipment.Assigns?.Any() != true)
        {
            throw new InvalidDataAppException("Selected equipment does not contain assigns");
        }

        var currentEquipmentAssign = equipment.Assigns.Find(assign => assign.ReturnDate is null)
            ?? throw new InvalidDataAppException("Selected equipment does not contain assigns without return date");

        var deletedEquipmentAssign = await _equipmentAssignRepository.DeleteEquipmentAssignmentAsync(
            currentEquipmentAssign.Id,
            cancellationToken);

        return _mapper.Map<EquipmentHolderDto>(deletedEquipmentAssign);
    }
}

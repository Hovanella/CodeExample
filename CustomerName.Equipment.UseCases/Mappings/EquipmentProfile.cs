using AutoMapper;
using CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipment;

namespace CustomerName.Portal.Equipment.UseCases.Mappings;

internal class EquipmentProfile : Profile
{
    public EquipmentProfile()
    {
        CreateMap<CreateEquipmentCommand, Domain.Equipment>()
            .ForMember(domainModel => domainModel.Approver, options => options.Ignore())
            .ForMember(domainModel => domainModel.Assigns, options => options.Ignore())
            .ForMember(domainModel => domainModel.Id, options => options.Ignore())
            .ForMember(domainModel => domainModel.Type, options => options.Ignore())
            .ForMember(domainModel => domainModel.Assigns, options => options.Ignore());
    }
}

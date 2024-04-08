using CustomerName.Portal.Equipment.Controllers.Contracts.Output;
using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.Controllers.Mappers;

public interface IActiveHolderDtoToEquipmentHolderOutputMapper
{
    EquipmentHolderOutput Map(ActiveHolderDto result);
}

public class ActiveHolderDtoToEquipmentHolderOutputMapper : IActiveHolderDtoToEquipmentHolderOutputMapper
{
    public EquipmentHolderOutput Map(ActiveHolderDto result)
    {
        return new EquipmentHolderOutput
        {
            Id = result.Id,
            FullName = $"{result.LastName} {result.FirstName}",
            DepartmentId = result.DepartmentId,
            IssueDate = result.IssueDate,
            ReturnDate = result.ReturnDate
        };
    }
}



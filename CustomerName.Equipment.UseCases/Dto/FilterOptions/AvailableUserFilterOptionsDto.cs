using CustomerName.Portal.Equipment.Domain;

namespace CustomerName.Portal.Equipment.UseCases.Dto.FilterOptions;

public record AvailableUserFilterOptionsDto(List<FilterUser> Approvers, List<FilterUser> Users);

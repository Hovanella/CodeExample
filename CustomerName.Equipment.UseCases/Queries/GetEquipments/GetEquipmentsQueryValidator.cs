using FluentValidation;
using CustomerName.Portal.Framework.UseCases.Abstractions.ValidationRules;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipments;

internal class GetEquipmentsQueryValidator : AbstractValidator<GetEquipmentsQuery>
{
    public GetEquipmentsQueryValidator()
    {
        RuleFor(x => x.QueryOptions).SetValidator(new ODataQueryOptionsValidator());
    }
}

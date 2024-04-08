using FluentValidation;
using CustomerName.Portal.Framework.UseCases.Abstractions.ValidationRules;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentsByFile;

internal class GetEquipmentsByFileQueryValidator : AbstractValidator<GetEquipmentsByFileQuery>
{
    public GetEquipmentsByFileQueryValidator()
    {
        RuleFor(x => x.Format).IsInEnum();
        RuleFor(x => x.QueryOptions).SetValidator(new ODataQueryOptionsValidator());
    }
}

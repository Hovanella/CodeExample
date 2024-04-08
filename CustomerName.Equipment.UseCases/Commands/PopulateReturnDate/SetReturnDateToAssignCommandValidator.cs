using FluentValidation;
using CustomerName.Portal.Framework.UseCases.Abstractions.ValidationRules;

namespace CustomerName.Portal.Equipment.UseCases.Commands.PopulateReturnDate;

internal class SetReturnDateToAssignCommandValidator : AbstractValidator<SetReturnDateToAssignCommand>
{
    public SetReturnDateToAssignCommandValidator()
    {
        RuleFor(x => x.EquipmentId)
            .GreaterThan(0);

        RuleFor(x => x.ReturnDate)
            .DateRule();
    }
}

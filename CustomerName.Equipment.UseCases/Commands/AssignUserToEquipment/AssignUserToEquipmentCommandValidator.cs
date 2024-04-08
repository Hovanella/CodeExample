using FluentValidation;
using CustomerName.Portal.Framework.UseCases.Abstractions.ValidationRules;

namespace CustomerName.Portal.Equipment.UseCases.Commands.AssignUserToEquipment;

internal class AssignUserToEquipmentCommandValidator : AbstractValidator<AssignUserToEquipmentCommand>
{
    public AssignUserToEquipmentCommandValidator()
    {
        RuleFor(x => x.EquipmentId)
            .GreaterThan(0);

        RuleFor(x => x.IssueDate)
            .DateRule();
    }
}

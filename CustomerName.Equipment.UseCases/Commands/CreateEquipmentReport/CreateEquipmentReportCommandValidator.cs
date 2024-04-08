using FluentValidation;

namespace CustomerName.Portal.Equipment.UseCases.Commands.CreateEquipmentReport;

internal class CreateEquipmentReportCommandValidator : AbstractValidator<CreateEquipmentReportCommand>
{
    public CreateEquipmentReportCommandValidator()
    {
        RuleFor(command => command.File)
            .Must(file => file.FileName.EndsWith("json"))
            .WithMessage("File extension is not json");
    }
}

using FluentValidation;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportById;

internal class GetEquipmentReportByIdQueryValidator : AbstractValidator<GetEquipmentReportByIdQuery>
{
    public GetEquipmentReportByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .GreaterThan(0);
    }
}

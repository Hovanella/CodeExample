using FluentValidation;

namespace CustomerName.Portal.Equipment.UseCases.Queries.GetEquipmentReportRelevancePeriods;

internal class GetEquipmentReportRelevancePeriodsQueryValidator :
    AbstractValidator<GetEquipmentReportRelevancePeriodsQuery>
{
    public GetEquipmentReportRelevancePeriodsQueryValidator()
    {
        RuleFor(query => query.SerialNumber)
            .NotEmpty();
    }
}

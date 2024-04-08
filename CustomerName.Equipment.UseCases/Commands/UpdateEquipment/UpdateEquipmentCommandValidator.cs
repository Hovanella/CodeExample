using FluentValidation;
using CustomerName.Portal.Framework.UseCases.Abstractions.ValidationRules;

namespace CustomerName.Portal.Equipment.UseCases.Commands.UpdateEquipment;

internal class UpdateEquipmentCommandValidator : AbstractValidator<UpdateEquipmentCommand>
{
    public UpdateEquipmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.GuaranteeDate)
            .NotNull()
            .DateRule();

        RuleFor(x => x.PurchaseDate)
            .NotNull()
            .DateRule();

        RuleFor(x => x.Name)
            .ShortTextType()
            .SpacesOrPunctuationSymbolsRule();

        RuleFor(x => x.SerialNumber)
            .ShortTextType()
            .SpacesOrPunctuationSymbolsRule();

        RuleFor(x => x.PurchasePrice).GreaterThanOrEqualTo(0).Money();
        RuleFor(x => x.PurchasePriceUsd).GreaterThanOrEqualTo(0).Money();
        RuleFor(x => x.PurchaseCurrency).IsInEnum();

        RuleFor(x => x.PurchasePlace)
            .NotEmpty()
            .AlphanumericWithSpecialChars()
            .SpacesOrPunctuationSymbolsRule();

        RuleFor(x => x.Characteristics)
            .LongTextType()
            .SpacesOrPunctuationSymbolsRule();

        RuleFor(x => x.ApproverId).GreaterThan(0);
        RuleFor(x => x.Location).IsInEnum();

        RuleFor(x => x.Comment)
            .BeEmptyOrBeLongTextType()
            .SpacesOrPunctuationSymbolsRule();

        RuleFor(x => x.InvoiceNumber)
            .ShortTextType()
            .SpacesOrPunctuationSymbolsRule();
    }
}

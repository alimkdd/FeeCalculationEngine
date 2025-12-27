using FeeCalculationEngine.Application.Dtos.Requests;
using FluentValidation;

namespace FeeCalculationEngine.Application.Validators;

public class FeeCalculationRequestValidator : AbstractValidator<FeeCalculationRequest>
{
    public FeeCalculationRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");

        RuleFor(x => x.TransactionTypeId)
            .GreaterThan(0)
            .WithMessage("TransactionTypeId must be greater than 0.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0)
            .WithMessage("PaymentMethodId must be greater than 0.");

        RuleFor(x => x.PromoCodeId)
            .GreaterThanOrEqualTo(0)
            .WithMessage("PromoCodeId must be 0 or greater.");
    }
}

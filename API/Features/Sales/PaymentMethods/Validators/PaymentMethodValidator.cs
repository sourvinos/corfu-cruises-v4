using FluentValidation;

namespace API.Features.Sales.PaymentMethods {

    public class PaymentMethodValidator : AbstractValidator<PaymentMethodWriteDto> {

        public PaymentMethodValidator() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.DescriptionEn).NotEmpty().MaximumLength(128);
        }

    }

}
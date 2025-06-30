using FluentValidation;

namespace API.Features.Sales.Banks {

    public class BankValidator : AbstractValidator<BankWriteDto> {

        public BankValidator() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}
using FluentValidation;

namespace API.Features.Sales.TaxOffices {

    public class TaxOfficeValidator : AbstractValidator<TaxOfficeWriteDto> {

        public TaxOfficeValidator() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}
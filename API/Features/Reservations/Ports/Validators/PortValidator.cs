using FluentValidation;

namespace API.Features.Reservations.Ports {

    public class PortValidator : AbstractValidator<PortWriteDto> {

        public PortValidator() {
            // Fields
            RuleFor(x => x.Abbreviation).NotEmpty().MaximumLength(5);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Locode).NotEmpty().Matches("[A-Z]{5}").Length(5);
            RuleFor(x => x.StopOrder).InclusiveBetween(1, to: 9);
        }

    }

}
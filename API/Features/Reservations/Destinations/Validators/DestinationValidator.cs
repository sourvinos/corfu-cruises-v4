using FluentValidation;

namespace API.Features.Reservations.Destinations {

    public class DestinationValidator : AbstractValidator<DestinationWriteDto> {

        public DestinationValidator() {
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Abbreviation).NotEmpty().MaximumLength(5);
        }

    }

}
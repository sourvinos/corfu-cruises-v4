using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.Reservations.PickupPoints {

    public class PickupPointValidator : AbstractValidator<PickupPointWriteDto> {

        public PickupPointValidator() {
            // FKs
            RuleFor(x => x.CoachRouteId).NotEmpty();
            RuleFor(x => x.PortId).NotEmpty();
            // Fields
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.ExactPoint).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Time).Must(TimeHelpers.BeValidTime);
            RuleFor(x => x.Remarks).MaximumLength(2048);
        }

    }

}
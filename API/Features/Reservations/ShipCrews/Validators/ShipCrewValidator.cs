using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.Reservations.ShipCrews {

    public class ShipCrewValidator : AbstractValidator<ShipCrewWriteDto> {

        public ShipCrewValidator() {
            // FKs
            RuleFor(x => x.GenderId).NotEmpty();
            RuleFor(x => x.NationalityId).NotEmpty();
            RuleFor(x => x.ShipId).NotEmpty();
            RuleFor(x => x.SpecialtyId).NotEmpty();
            // Fields
            RuleFor(x => x.Lastname).NotEmpty().Matches("^[a-zA-Z]+([ a-zA-Z]+)?$").MaximumLength(128);
            RuleFor(x => x.Firstname).NotEmpty().Matches("^[a-zA-Z]+([ a-zA-Z]+)?$").MaximumLength(128);
            RuleFor(x => x.Birthdate).Must(DateHelpers.BeCorrectFormat).Must(DateHelpers.AgeMustBeMaxOneHundredYears);
        }

    }

}
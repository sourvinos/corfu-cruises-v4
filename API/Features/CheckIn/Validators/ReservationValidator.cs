using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.CheckIn {

    public class CheckInValidator : AbstractValidator<ReservationWriteDto> {

        public CheckInValidator() {
            // Passengers
            RuleForEach(x => x.Passengers).ChildRules(passenger => {
                passenger.RuleFor(x => x.GenderId).NotEmpty();
                passenger.RuleFor(x => x.NationalityId).NotEmpty();
                passenger.RuleFor(x => x.Lastname).NotEmpty().MaximumLength(128);
                passenger.RuleFor(x => x.Firstname).NotEmpty().MaximumLength(128);
                passenger.RuleFor(x => x.Birthdate).Must(DateHelpers.BeCorrectFormat);
                passenger.RuleFor(x => x.Remarks).MaximumLength(128);
                passenger.RuleFor(x => x.SpecialCare).MaximumLength(128);
            });
        }

    }

    public class PassengerValidator : AbstractValidator<PassengerWriteDto> {

        public PassengerValidator() {
            RuleFor(x => x.NationalityId).NotEmpty();
            RuleFor(x => x.GenderId).NotEmpty();
            RuleFor(x => x.Firstname).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Lastname).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Birthdate).Must(DateHelpers.BeCorrectFormat);
            RuleFor(x => x.SpecialCare).MaximumLength(128);
            RuleFor(x => x.Remarks).MaximumLength(128);
        }

    }

}
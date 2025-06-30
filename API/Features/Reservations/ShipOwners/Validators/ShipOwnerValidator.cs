using API.Infrastructure.Helpers;
using FluentValidation;

namespace API.Features.Reservations.ShipOwners {

    public class ShipOwnerValidator : AbstractValidator<ShipOwnerWriteDto> {

        public ShipOwnerValidator() {
            // FKs
            RuleFor(x => x.NationalityId).NotEmpty();
            RuleFor(x => x.TaxOfficeId).NotEmpty();
            // Fields
            RuleFor(x => x.VatPercent).GreaterThanOrEqualTo(0);
            RuleFor(x => x.VatPercentId).InclusiveBetween(1, 9); ;
            RuleFor(x => x.VatExemptionId).InclusiveBetween(0, 30); ;
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.DescriptionEn).NotEmpty().MaximumLength(128);
            RuleFor(x => x.VatNumber).NotEmpty().MaximumLength(36);
            RuleFor(x => x.Branch).InclusiveBetween(0, 10);
            RuleFor(x => x.Profession).MaximumLength(128);
            RuleFor(x => x.Street).MaximumLength(128);
            RuleFor(x => x.Number).MaximumLength(4);
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(10);
            RuleFor(x => x.City).NotEmpty().MaximumLength(128);
            RuleFor(x => x.PersonInCharge).MaximumLength(128);
            RuleFor(x => x.Phones).MaximumLength(128);
            RuleFor(x => x.Email).Must(EmailHelpers.BeEmptyOrValidEmailAddress).MaximumLength(128);
        }

    }

}
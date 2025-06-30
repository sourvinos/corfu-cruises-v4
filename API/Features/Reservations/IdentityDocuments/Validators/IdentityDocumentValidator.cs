using FluentValidation;

namespace API.Features.Reservations.IdentityDocuments {

    public class IdentityDocumentValidator : AbstractValidator<IdentityDocumentWriteDto> {

        public IdentityDocumentValidator() {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
        }

    }

}
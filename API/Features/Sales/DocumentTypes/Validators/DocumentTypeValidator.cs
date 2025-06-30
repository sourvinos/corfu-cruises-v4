using FluentValidation;

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeValidator : AbstractValidator<DocumentTypeWriteDto> {

        public DocumentTypeValidator() {
            // FKs
            RuleFor(x => x.ShipOwnerId).NotEmpty();
            // Fields
            RuleFor(x => x.Abbreviation).NotEmpty().MaximumLength(5);
            RuleFor(x => x.AbbreviationEn).NotEmpty().MaximumLength(5);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Batch).NotNull().MaximumLength(5);
            RuleFor(x => x.BatchEn).NotNull().MaximumLength(5);
            RuleFor(x => x.DiscriminatorId).NotNull().InclusiveBetween(1, 3);
            RuleFor(x => x.Customers).NotNull().MaximumLength(1).Matches(@"^[+|\-| ]*$");
            RuleFor(x => x.Suppliers).NotNull().MaximumLength(1).Matches(@"^[+|\-| ]*$");
            RuleFor(x => x.Table8_1).NotNull().MaximumLength(32);
            RuleFor(x => x.Table8_8).NotNull().MaximumLength(32);
            RuleFor(x => x.Table8_9).NotNull().MaximumLength(32);
        }

    }

}
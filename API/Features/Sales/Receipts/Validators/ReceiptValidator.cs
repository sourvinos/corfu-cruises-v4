using FluentValidation;

namespace API.Features.Sales.Receipts {

    public class ReceiptValidator : AbstractValidator<ReceiptWriteDto> {

        public ReceiptValidator() {
            // FKs
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.DocumentTypeId).NotEmpty();
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.ShipOwnerId).NotEmpty();
            // Fields
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.InvoiceNo).NotEmpty();
            RuleFor(x => x.GrossAmount).InclusiveBetween(0, 99999);
            RuleFor(x => x.Remarks).MaximumLength(128);
        }

    }

}
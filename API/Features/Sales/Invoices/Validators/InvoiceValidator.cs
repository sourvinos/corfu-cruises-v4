using FluentValidation;

namespace API.Features.Sales.Invoices {

    public class InvoiceValidator : AbstractValidator<InvoiceWriteDto> {

        public InvoiceValidator() {
            // FKs
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.DestinationId).NotEmpty();
            RuleFor(x => x.DocumentTypeId).NotEmpty();
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.ShipId).NotEmpty();
            // Fields
            RuleFor(x => x.Remarks).MaximumLength(128);
            // Ports
            RuleForEach(x => x.InvoicesPorts).ChildRules(port => {
                port.RuleFor(x => x.PortId).NotEmpty();
                port.RuleFor(x => x.AdultsWithTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.AdultsPriceWithTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.AdultsWithoutTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.AdultsPriceWithoutTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.KidsWithTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.KidsPriceWithTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.KidsWithoutTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.KidsPriceWithoutTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.FreeWithTransfer).InclusiveBetween(0, 999);
                port.RuleFor(x => x.FreeWithoutTransfer).InclusiveBetween(0, 999);
            });
        }

    }

}
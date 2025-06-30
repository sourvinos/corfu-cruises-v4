namespace API.Features.Sales.Invoices {

    public class InvoiceCreateDto : InvoiceWriteDto {

        public InvoiceAade Aade { get; set; } = new InvoiceAade();

    }

}
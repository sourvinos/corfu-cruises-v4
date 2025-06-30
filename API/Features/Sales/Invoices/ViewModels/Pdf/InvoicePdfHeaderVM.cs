namespace API.Features.Sales.Invoices {

    public class InvoicePdfHeaderVM {

        public string Date { get; set; }
        public string TripDate { get; set; }
        public InvoicePdfDocumentTypeVM DocumentType { get; set; }
        public int InvoiceNo { get; set; }

    }

}
namespace API.Features.Sales.Receipts {

    public class ReceiptPdfHeaderVM {

        public string Date { get; set; }
        public string TripDate { get; set; }
        public ReceiptPdfDocumentTypeVM DocumentType { get; set; }
        public int InvoiceNo { get; set; }

    }

}
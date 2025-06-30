using System;
using API.Infrastructure.Classes;

namespace API.Features.Sales.Receipts {

    public class ReceiptPdfVM {

        public Guid InvoiceId { get; set; }
        public ReceiptPdfHeaderVM Header { get; set; }
        public string Remarks { get; set; }
        public ReceiptPdfPartyVM Customer { get; set; }
        public ReceiptPdfDocumentTypeVM DocumentType { get; set; }
        public string PaymentMethod { get; set; }
        public ReceiptPdfPartyVM Issuer { get; set; }
        public InvoicePdfSummaryVM Summary { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }
        public SimpleEntity[] BankAccounts { get; set; }

    }

}
using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.Sales.Invoices {

    public class InvoicePdfVM {

        public Guid InvoiceId { get; set; }
        public InvoicePdfHeaderVM Header { get; set; }
        public string Remarks { get; set; }
        public InvoicePdfShipVM Ship { get; set; }
        public InvoicePdfAadeVM Aade { get; set; }
        public List<InvoicePdfPortVM> Ports { get; set; }
        public InvoicePdfPartyVM Customer { get; set; }
        public string Destination { get; set; }
        public InvoicePdfDocumentTypeVM DocumentType { get; set; }
        public string PaymentMethod { get; set; }
        public InvoicePdfPartyVM Issuer { get; set; }
        public InvoicePdfSummaryVM Summary { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }
        public SimpleEntity[] BankAccounts { get; set; }

    }

}
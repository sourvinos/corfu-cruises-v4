using System;

namespace Invoices {

    public class TestInvoice {

        public int StatusCode { get; set; }

        // PK
        public Guid? InvoiceId { get; set; }
        // FKs
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public int ShipId { get; set; }
        // Fields
        public string Date { get; set; }
        public string TripDate { get; set; }
        public int InvoiceNo { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }

    }

}
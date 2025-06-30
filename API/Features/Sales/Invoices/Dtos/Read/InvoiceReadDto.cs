using System;
using System.Collections.Generic;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Invoices {

    public class InvoiceReadDto : IMetadata {

        // PK
        public Guid InvoiceId { get; set; }
        // Fields
        public DateTime Date { get; set; }
        public DateTime TripDate { get; set; }
        public int InvoiceNo { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsCancelled { get; set; }
        // Child tables
        public InvoiceFormAadeVM Aade { get; set; }
        public List<InvoicePortReadDto> InvoicesPorts { get; set; }
        // Navigation
        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public DocumentTypeVM DocumentType { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        public SimpleEntity Ship { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}
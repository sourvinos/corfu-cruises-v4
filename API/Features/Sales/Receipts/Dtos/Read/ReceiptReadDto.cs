using System;
using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Receipts {

    public class ReceiptReadDto : IMetadata {

        // PK
        public Guid InvoiceId { get; set; }
        // FKs
        public int CustomerId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        // Fields
        public string Date { get; set; }
        public string TripDate { get; set; }
        public int InvoiceNo { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsCancelled { get; set; }
        // Navigation
        public SimpleEntity Customer { get; set; }
        public DocumentTypeVM DocumentType { get; set; }
        public SimpleEntity PaymentMethod { get; set; }
        public SimpleEntity ShipOwner { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}
using System;
using API.Features.Sales.DocumentTypes;
using API.Features.Sales.PaymentMethods;
using API.Features.Reservations.Customers;
using API.Features.Reservations.ShipOwners;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.Transactions {

    public abstract class TransactionsBase : IMetadata {

        // PK
        public Guid InvoiceId { get; set; }
        // FKs
        public int CustomerId { get; set; }
        public int DocumentTypeId { get; set; }
        public int PaymentMethodId { get; set; }
        public int ShipOwnerId { get; set; }
        public int DiscriminatorId { get; set; }
        // Fields
        public DateTime Date { get; set; }
        public DateTime TripDate { get; set; }
        public int InvoiceNo { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal NewBalance { get; set; }
        public string Remarks { get; set; }
        public bool IsEmailPending { get; set; }
        public bool IsEmailSent { get; set; }
        public bool IsCancelled { get; set; }
        // Navigation
        public DocumentType DocumentType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Customer Customer { get; set; }
        public ShipOwner ShipOwner { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
    }

}
using API.Features.Sales.Banks;
using API.Features.Reservations.ShipOwners;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.BankAccounts {

    public class BankAccount : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int ShipOwnerId { get; set; }
        public int BankId { get; set; }
        // Fields
        public string Iban { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public ShipOwner ShipOwner { get; set; }
        public Bank Bank { get; set; }

    }

}
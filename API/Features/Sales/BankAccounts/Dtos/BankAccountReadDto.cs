using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.BankAccounts {

    public class BankAccountReadDto : IMetadata {

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
        public SimpleEntity ShipOwner { get; set; }
        public SimpleEntity Bank { get; set; }

    }

}
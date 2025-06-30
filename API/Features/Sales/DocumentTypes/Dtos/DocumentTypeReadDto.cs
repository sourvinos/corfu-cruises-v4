using API.Infrastructure.Classes;
using API.Infrastructure.Interfaces;

namespace API.Features.Sales.DocumentTypes {

    public class DocumentTypeReadDto : IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public SimpleEntity Ship { get; set; }
        public SimpleEntity ShipOwner { get; set; }
        // Fields
        public string Abbreviation { get; set; }
        public string AbbreviationEn { get; set; }
        public string Description { get; set; }
        public string Batch { get; set; }
        public string BatchEn { get; set; }
        public string Customers { get; set; }
        public string Suppliers { get; set; }
        public int DiscriminatorId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public bool IsMyData { get; set; }
        public string Table8_1 { get; set; }
        public string Table8_8 { get; set; }
        public string Table8_9 { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}
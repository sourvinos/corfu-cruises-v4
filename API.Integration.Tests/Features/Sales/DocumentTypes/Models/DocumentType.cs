namespace DocumentTypes {

    public class TestDocumentType {

        public int StatusCode { get; set; }

        // PK
        public int Id { get; set; }
        // FKs
        public int? ShipId { get; set; }
        public int ShipOwnerId { get; set; }
        // Fields
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public string Batch { get; set; }
        public string LastDate { get; set; }
        public int LastNo { get; set; }
        public string Customers { get; set; }
        public string Suppliers { get; set; }
        public int DiscriminatorId { get; set; }
        public bool IsActive { get; set; }
        // myData
        public bool IsMyData { get; set; }
        public string Table8_1 { get; set; }
        public string Table8_8 { get; set; }
        public string Table8_9 { get; set; }
        // Metadata
        public string PutAt { get; set; }

    }

}
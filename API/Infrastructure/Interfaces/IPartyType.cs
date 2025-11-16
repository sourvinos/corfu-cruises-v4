namespace API.Infrastructure.Interfaces {

    public interface IPartyType : IBaseEntity, IMetadata {

        // Fks
        public int NationalityId { get; set; }
        public int TaxOfficeId { get; set; }
        // Fields
        public string VatNumber { get; set; }
        public int Branch { get; set; }
        public string Profession { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Phones { get; set; }
        public string PersonInCharge { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }

}
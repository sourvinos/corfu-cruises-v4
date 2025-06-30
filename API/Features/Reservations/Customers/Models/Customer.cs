using API.Features.Reservations.Nationalities;
using API.Features.Sales.TaxOffices;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Customers {

    public class Customer : IPartyType {

        // PK
        public int Id { get; set; }
        // Fks
        public int NationalityId { get; set; }
        public int TaxOfficeId { get; set; }
        // Fields
        public decimal VatPercent { get; set; }
        public int VatPercentId { get; set; }
        public int VatExemptionId { get; set; }
        public string Description { get; set; }
        public string FullDescription { get; set; }
        public string VatNumber { get; set; }
        public int Branch { get; set; }
        public string Profession { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string PersonInCharge { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        public decimal BalanceLimit { get; set; }
        public int PaxLimit { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public Nationality Nationality { get; set; }
        public TaxOffice TaxOffice { get; set; }

    }

}
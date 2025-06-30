using System.Collections.Generic;
using API.Features.Sales.BankAccounts;
using API.Features.Sales.TaxOffices;
using API.Features.Reservations.Nationalities;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.ShipOwners {

    public class ShipOwner : IPartyType {

        // PK
        public int Id { get; set; }
        // FKs
        public int NationalityId { get; set; }
        public int TaxOfficeId { get; set; }
        // Fields
        public decimal VatPercent { get; set; }
        public int VatPercentId { get; set; }
        public int VatExemptionId { get; set; }
        public string Description { get; set; }
        public string DescriptionEn { get; set; }
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
        public bool IsGroupJP { get; set; }
        public bool IsActive { get; set; }
        // myData
        public string MyDataDemoUrl { get; set; }
        public string MyDataDemoUsername { get; set; }
        public string MyDataDemoSubscriptionKey { get; set; }
        public string MyDataLiveUrl { get; set; }
        public string MyDataLiveUsername { get; set; }
        public string MyDataLiveSubscriptionKey { get; set; }
        public bool MyDataIsDemo { get; set; }
        public bool MyDataIsActive { get; set; }
        // Oxygen
        public string OxygenDemoUrl { get; set; }
        public string OxygenDemoAPIKey { get; set; }
        public string OxygenLiveUrl { get; set; }
        public string OxygenLiveAPIKey { get; set; }
        public bool OxygenIsDemo { get; set; }
        public bool OxygenIsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public Nationality Nationality { get; set; }
        public TaxOffice TaxOffice { get; set; }
        public List<BankAccount> BankAccounts { get; set; }

    }

}
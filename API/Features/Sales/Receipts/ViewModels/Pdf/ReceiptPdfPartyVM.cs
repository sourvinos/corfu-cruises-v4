namespace API.Features.Sales.Receipts {

    public class ReceiptPdfPartyVM {

        public int Id { get; set; }
        public string FullDescription { get; set; }
        public string Nationality { get; set; }
        public string TaxOffice { get; set; }
        public string VatNumber { get; set; }
        public int Branch { get; set; }
        public string Profession { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }

    }

}
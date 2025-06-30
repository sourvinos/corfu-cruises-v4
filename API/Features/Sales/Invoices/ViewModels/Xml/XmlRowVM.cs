namespace API.Features.Sales.Invoices {

    public class XmlRowVM {

        public int LineNumber { get; set; }
        public decimal NetValue { get; set; }
        public int VatCategory { get; set; }
        public decimal VatAmount { get; set; }
        public int VatExemptionCategory { get; set; }

    }

}
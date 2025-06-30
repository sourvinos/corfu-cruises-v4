namespace API.Features.Sales.Invoices {

    public class XmlSummaryVM {

        public decimal TotalNetValue { get; set; }
        public decimal TotalVatAmount { get; set; }
        public decimal TotalWithheldAmount { get; set; }
        public decimal TotalFeesAmount { get; set; }
        public decimal TotalStampDutyAmount { get; set; }
        public decimal TotalOtherTaxesAmount { get; set; }
        public decimal TotalDeductionsAmount { get; set; }
        public decimal TotalGrossValue { get; set; }
        public XmlIncomeClassificationVM IncomeClassification { get; set; }

    }

}
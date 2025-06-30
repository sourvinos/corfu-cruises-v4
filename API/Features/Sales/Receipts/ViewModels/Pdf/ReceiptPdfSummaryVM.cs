namespace API.Features.Sales.Receipts {

    public class InvoicePdfSummaryVM {

        public decimal NetAmount { get; set; }
        public decimal VatPercent { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }

    }

}
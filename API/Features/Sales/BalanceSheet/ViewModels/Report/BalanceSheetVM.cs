using API.Infrastructure.Classes;

namespace API.Features.Sales.BalanceSheet {

    public class BalanceSheetVM {

        public string Date { get; set; }
        public SimpleEntity Customer { get; set; }
        public string InvoiceNo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }

    }

}
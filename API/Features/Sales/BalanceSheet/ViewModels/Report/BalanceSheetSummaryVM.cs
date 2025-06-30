using API.Infrastructure.Classes;

namespace API.Features.Sales.BalanceSheet {

    public class BalanceSheetSummaryVM {

        public SimpleEntity ShipOwner { get; set; }
        public SimpleEntity Customer { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public decimal ActualBalance { get; set; }

    }

}
namespace API.Features.Sales.BalanceSheet {

    public class BalanceSheetCriteria {

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int? ShipOwnerId { get; set; }

    }

}
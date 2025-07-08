using System;

namespace API.Features.Sales.Ledgers {

    public class LedgerCriteria {

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CustomerId { get; set; }
        public int? ShipOwnerId { get; set; }

    }

}
using System;

namespace API.Infrastructure.EmailServices {

    public class EmailLedgerSaleQueue {

        public int Id { get; set; }
        public Guid EntityId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CustomerId { get; set; }
        public int ShipOwnerId { get; set; }

    }

}
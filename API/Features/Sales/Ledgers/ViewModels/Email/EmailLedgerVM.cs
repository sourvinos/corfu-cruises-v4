using System.Collections.Generic;

namespace API.Features.Sales.Ledgers {

    public class EmailLedgerVM {

        public int CustomerId { get; set; }
        public IList<string> Filenames { get; set; }

    }

}
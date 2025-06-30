using System.Collections.Generic;
using API.Features.Sales.Transactions;

namespace API.Features.Sales.Invoices {

    public class InvoiceWriteDto : TransactionsBase {

        public int? DestinationId { get; set; }
        public int? ShipId { get; set; }
        public List<InvoicePortWriteDto> InvoicesPorts { get; set; }

    }

}
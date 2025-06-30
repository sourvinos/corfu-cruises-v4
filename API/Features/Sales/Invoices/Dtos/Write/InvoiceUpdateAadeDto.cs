using System;

namespace API.Features.Sales.Invoices {

    public class InvoiceUpdateAadeDto {

        public Guid InvoiceId { get; set; }
        public string Uid { get; set; }
        public string Mark { get; set; }
        public string MarkCancel { get; set; }
        public string Url { get; set; }

    }

}
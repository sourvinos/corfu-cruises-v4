using System;

namespace API.Features.Sales.Invoices {

    public class XmlAadeVM {

        public Guid InvoiceId { get; set; }
        public string UId { get; set; }
        public string Mark { get; set; }
        public string MarkCancel { get; set; }
        public string Url { get; set; }

    }

}
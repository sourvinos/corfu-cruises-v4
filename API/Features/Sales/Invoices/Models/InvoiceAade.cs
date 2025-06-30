using System;

namespace API.Features.Sales.Invoices {

    public class InvoiceAade {

        public Guid InvoiceId { get; set; }
        public string Id { get; set; }
        public string Uid { get; set; }
        public string Mark { get; set; }
        public string MarkCancel { get; set; }
        public string AuthenticationCode { get; set; }
        public string ICode { get; set; }
        public string Url { get; set; }
        public string Discriminator { get; set; }

    }

}
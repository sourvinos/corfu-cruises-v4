using System;

namespace API.Features.Sales.Invoices {

    public class XmlBuilderVM {

        public Guid InvoiceId { get; set; }
        public XmlCredentialsVM Credentials { get; set; }
        public XmlPartyVM Issuer { get; set; }
        public XmlPartyVM CounterPart { get; set; }
        public XmlHeaderVM InvoiceHeader { get; set; }
        public XmlPaymentMethodVM PaymentMethod { get; set; }
        public XmlRowVM InvoiceDetail { get; set; }
        public XmlSummaryVM InvoiceSummary { get; set; }
        public XmlAadeVM Aade { get; set; }

    }

}
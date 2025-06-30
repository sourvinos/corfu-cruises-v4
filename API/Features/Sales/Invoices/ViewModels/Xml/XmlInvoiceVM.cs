using System;

namespace API.Features.Sales.Invoices {

    public class XmlInvoiceVM {

        public Guid InvoiceId { get; set; }
        public XmlCredentialsVM Credentials { get; set; }
        public XmlIssuerVM Issuer { get; set; }
        public XmlCounterPartVM CounterPart { get; set; }
        public XmlHeaderVM InvoiceHeader { get; set; }
        public XmlPaymentMethodVM PaymentMethod { get; set; }
        public XmlRowVM InvoiceDetail { get; set; }
        public XmlSummaryVM InvoiceSummary { get; set; }
        public XmlAadeVM Aade { get; set; }

    }

}
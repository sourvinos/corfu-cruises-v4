using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonInvoiceVM {

        [JsonProperty("issuer")]
        public JsonIssuerVM Issuer { get; set; }

        [JsonProperty("counterpart")]
        public JsonCounterPartVM CounterPart { get; set; }

        [JsonProperty("header")]
        public JsonHeaderVM Header { get; set; }

        [JsonProperty("payment_methods")]
        public List<JsonPaymentMethodDetailVM> Payment_Methods { get; set; }

        [JsonProperty("lines")]
        public List<JsonLineVM> Lines { get; set; }

        [JsonProperty("summary")]
        public JsonSummaryVM Summary { get; set; }

        [JsonProperty("options")]
        public JsonOptionsVM Options { get; set; }

    }

}
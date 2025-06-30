using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonHeaderVM {

        [JsonProperty("series")]
        public string Series { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("issued_at")]
        public string Issued_At { get; set; }

        [JsonProperty("invoice_type")]
        public string Invoice_Type { get; set; }

    }

}
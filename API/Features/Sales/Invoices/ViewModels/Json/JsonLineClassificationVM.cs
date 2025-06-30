using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonLineClassificationVM {

        [JsonProperty("line_number")]
        public int LineNumber { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("type")]
        public decimal Type { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

    }

}
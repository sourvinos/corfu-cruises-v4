using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonSummaryClassificationVM {

        [JsonProperty("category")]
        public string Category { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

    }

}
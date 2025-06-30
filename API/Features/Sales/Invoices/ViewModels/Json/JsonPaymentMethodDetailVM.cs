using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonPaymentMethodDetailVM {

        [JsonProperty("type")]
        public int Type { get; set; }
        
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

    }

}
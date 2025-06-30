using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonSummaryVM {

        [JsonProperty("total_net_amount")]
        public decimal Total_Net_Amount { get; set; }
        
        [JsonProperty("total_vat_amount")]
        public decimal Total_Vat_Amount { get; set; }
        
        [JsonProperty("total_gross_amount")]
        public decimal Total_Gross_Amount { get; set; }
        
        [JsonProperty("classifications")]
        public List<JsonSummaryClassificationVM> Classifications { get; set; }

    }

}
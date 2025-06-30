using System.Collections.Generic;
using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonLineVM {

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit_price")]
        public decimal Unit_Price { get; set; }

        [JsonProperty("net_amount")]
        public decimal Net_Amount { get; set; }

        [JsonProperty("vat_category")]
        public int Vat_Category { get; set; }

        [JsonProperty("vat_amount")]
        public decimal Vat_Amount { get; set; }

        [JsonProperty("total_amount")]
        public decimal Total_Amount { get; set; }

        [JsonProperty("vat_exemption_reason_code")]
        public string Vat_Exemption_Reason_Code { get; set; }

        [JsonProperty("classifications")]
        public List<JsonSummaryClassificationVM> Classifications { get; set; }

    }

}
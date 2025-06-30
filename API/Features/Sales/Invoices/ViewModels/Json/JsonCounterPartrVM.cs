using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonCounterPartVM {


        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vat_number")]
        public string Vat_Number { get; set; }

        [JsonProperty("branch_code")]
        public string Branch_Code { get; set; }

        [JsonProperty("country_code")]
        public string Country_Code { get; set; }

        [JsonProperty("address")]
        public JsonAddressVM Address { get; set; }

    }

}
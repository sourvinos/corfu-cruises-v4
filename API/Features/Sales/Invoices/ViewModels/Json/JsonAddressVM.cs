using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonAddressVM {

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("postal_code")]
        public string Postal_Code { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

    }

}
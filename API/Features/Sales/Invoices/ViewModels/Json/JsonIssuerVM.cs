using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonIssuerVM {

        [JsonProperty("vat_number")]
        public string Vat_Number { get; set; }
        
        [JsonProperty("branch_code")]
        public string Branch_Code { get; set; }

    }

}
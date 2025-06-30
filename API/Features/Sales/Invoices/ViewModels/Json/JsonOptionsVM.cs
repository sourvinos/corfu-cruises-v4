using Newtonsoft.Json;

namespace API.Features.Sales.Invoices {

    public class JsonOptionsVM {

        [JsonProperty("is_peppol")]
        public bool Is_Peppol { get; set; }

        [JsonProperty("ignore_classifications")]
        public bool Ignore_Classifications { get; set; }

    }

}
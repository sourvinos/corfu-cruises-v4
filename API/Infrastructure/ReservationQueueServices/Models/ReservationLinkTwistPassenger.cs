using System.Text.Json.Serialization;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationLinkTwistPassenger {

        [JsonPropertyName("surname")]
        public string Lastname { get; set; } = "";

        [JsonPropertyName("name")]
        public string Firstname { get; set; } = "";

        [JsonPropertyName("date_of_birth")]
        public string Birthdate { get; set; } = "";

        [JsonPropertyName("country")]
        public string Nationality { get; set; } = "";

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = "";

    }

}
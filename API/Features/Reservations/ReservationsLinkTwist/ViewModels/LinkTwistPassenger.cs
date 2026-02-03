using System.Text.Json.Serialization;

namespace API.Features.Reservations.LinkTwist {

    public class LinkTwistPassenger {

        [JsonPropertyName("surname")]
        public string Lastname { get; set; } = "";

        [JsonPropertyName("name")]
        public string Firstname { get; set; } = "";

        [JsonPropertyName("type")]
        public string Age { get; set; } = "0";

        [JsonPropertyName("date_of_birth")]
        public string Birthdate { get; set; } = "";

        [JsonPropertyName("country")]
        public string Nationality { get; set; } = "";

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = "";

    }

}
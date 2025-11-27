using System.Text.Json.Serialization;

namespace API.Features.Reservations.Reservations {

    public class LinkTwistPassenger {

        [JsonPropertyName("type")]
        public string Age { get; set; }

        [JsonPropertyName("surname")]
        public string Lastname { get; set; }

        [JsonPropertyName("name")]
        public string Firstname { get; set; }

        [JsonPropertyName("date_of_birth_(day/month/year)")]
        public string Birthdate { get; set; }

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

    }

}
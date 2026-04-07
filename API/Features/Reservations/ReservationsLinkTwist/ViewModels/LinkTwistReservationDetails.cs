using System.Text.Json.Serialization;

namespace API.Features.Reservations.LinkTwist {

    public class LinkTwistReservationDetails {

        [JsonPropertyName("activity_date_time")]
        public string Date { get; set; }

        [JsonPropertyName("participant_type_alias")]
        public string Age { get; set; }

        [JsonPropertyName("product_channel_alias")]
        public string Destination { get; set; }

        [JsonPropertyName("product_option_title")]
        public string Port { get; set; }

        [JsonPropertyName("contact_data")]
        public LinkTwistPassenger Passenger { get; set; }

    }

}
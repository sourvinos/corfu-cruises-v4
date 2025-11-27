using System.Text.Json.Serialization;

namespace API.Features.Reservations.Reservations {

    public class LinkTwistReservationDetails {

        [JsonPropertyName("activity_date_time")]
        public string Date { get; set; }

        [JsonPropertyName("product_id")]
        public int DestinationId { get; set; }

        [JsonPropertyName("contact_data")]
        public LinkTwistPassenger Passenger { get; set; }

    }

}
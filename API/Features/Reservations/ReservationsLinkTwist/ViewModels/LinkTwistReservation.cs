using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Features.Reservations.Reservations {

    public class LinkTwistReservation {

        [JsonPropertyName("code")]
        public string Code { get; set; }

        public string Date { get; set; }
        public string Destination { get; set; }

        [JsonPropertyName("referer")]
        public string Customer { get; set; }

        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Free { get; set; }

        public int TotalPax { get; set; }

        [JsonPropertyName("comments")]
        public object Comments { get; set; }

        [JsonPropertyName("booking_status")]
        public string Status { get; set; }

        [JsonPropertyName("items")]
        public List<LinkTwistReservationDetails> Details { get; set; }

        [JsonPropertyName("extras")]
        public List<LinkTwistPickupPoint> PickupPoint { get; set; }

    }

}
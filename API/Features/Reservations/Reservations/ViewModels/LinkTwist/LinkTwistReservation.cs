using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Features.Reservations.Reservations {

    public class LinkTwistReservation {

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("referer")]
        public string Customer { get; set; }

        [JsonPropertyName("comments")]
        public object Comments { get; set; }

        [JsonPropertyName("cancelled_at")]
        public string CancelledAt { get; set; }

        public bool IsCancelled { get; set; }

        [JsonPropertyName("items")]
        public List<LinkTwistReservationDetails> Details { get; set; }

        [JsonPropertyName("extras")]
        public List<LinkTwistPickupPoint> PickupPoint { get; set; } 

    }

}
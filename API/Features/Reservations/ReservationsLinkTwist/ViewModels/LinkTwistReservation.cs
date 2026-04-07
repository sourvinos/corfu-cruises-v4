using System.Collections.Generic;
using System.Text.Json.Serialization;
using API.Infrastructure.Classes;

namespace API.Features.Reservations.LinkTwist {

    public class LinkTwistReservation {

        public string Date { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("channel_booking_code")]
        public string BookingCode { get; set; }

        [JsonPropertyName("referer")]
        public string Referer { get; set; }

        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public SimpleEntity Port { get; set; }

        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Free { get; set; }
        public int TotalPax { get; set; }

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        [JsonPropertyName("booking_status")]
        public string BookingStatus { get; set; }
        public SimpleEntity Status { get; set; }

        [JsonPropertyName("items")]
        public List<LinkTwistReservationDetails> Details { get; set; }

        [JsonPropertyName("extras")]
        public List<LinkTwistPickupPoint> Extras { get; set; }

        public SimpleEntity PickupPoint { get; set; }

        public bool IsValidPrimary { get; set; }
        public bool IsValidSecondary { get; set; }

    }

}
using System.Collections.Generic;
using System.Text.Json.Serialization;
using API.Infrastructure.Classes;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationLinkTwist {

        public string Date { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("channel_booking_code")]
        public string BookingCode { get; set; }

        [JsonPropertyName("referer")]
        public string Referer { get; set; }

        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public SimpleEntity OurDestination { get; set; }
        public SimpleEntity Port { get; set; }

        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Free { get; set; }
        public int TotalPax { get; set; }

        [JsonPropertyName("comments")]
        public string Notes { get; set; }

        [JsonPropertyName("booking_status")]
        public string BookingStatus { get; set; }
        public SimpleEntity Status { get; set; }

        [JsonPropertyName("items")]
        public List<LinkTwistReservationDetails> Details { get; set; }

        [JsonPropertyName("extras")]
        public List<ReservationLinkTwistPickupPoint> Extras { get; set; }

        public SimpleEntity PickupPoint { get; set; }

        public bool IsValidPrimary { get; set; }
        public bool IsValidSecondary { get; set; }
        public bool Exists { get; set; }

    }

}
using System.Text.Json.Serialization;

namespace API.Infrastructure.ReservationQueueServices {

    public class ReservationLinkTwistPickupPoint {

        [JsonPropertyName("extra_alias")]
        public string Description { get; set; }

    }

}
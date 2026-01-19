using System.Text.Json.Serialization;

namespace API.Features.Reservations.PickupPointsLinkTwist {

    public class PickupPointLinkTwistVM {

        [JsonPropertyName("alias")]
        public string Alias { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

    }

}
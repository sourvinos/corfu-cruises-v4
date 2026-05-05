using System.Text.Json.Serialization;

namespace API.Features.Reservations.LinkTwist {

    public class LinkTwistPickupPoint {

        [JsonPropertyName("extra_alias")]
        public string Description { get; set; }

    }

}
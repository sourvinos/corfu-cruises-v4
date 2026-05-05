using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Parameters {

    public class ParameterReadDto : IMetadata {

        // PK
        public Guid Id { get; set; }
        // Fields
        public string ClosingTime { get; set; }
        public string Phones { get; set; }
        public string Email { get; set; }
        // LinkTwist
        public string LinkTwistDemoUrl { get; set; }
        public string LinkTwistDemoAPIKey { get; set; }
        public string LinkTwistLiveUrl { get; set; }
        public string LinkTwistLiveAPIKey { get; set; }
        public bool LinkTwistIsDemo { get; set; }
        public bool LinkTwistIsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}
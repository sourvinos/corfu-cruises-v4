using API.Features.Reservations.CoachRoutes;
using API.Features.Reservations.Ports;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.PickupPoints {

    public class PickupPoint : IBaseEntity, IMetadata {

        // PK
        public int Id { get; set; }
        // FKs
        public int CoachRouteId { get; set; }
        public int PortId { get; set; }
        // Fields
        public string Description { get; set; }
        public string ExactPoint { get; set; }
        public string Time { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        // Metadata
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }
        // Navigation
        public CoachRoute CoachRoute { get; set; }
        public Port Port { get; set; }

    }

}
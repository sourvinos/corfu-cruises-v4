using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Reservations {

    public class CloneReservationVM : IMetadata {

        public string Date { get; set; }
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int PickupPointId { get; set; }
        public int PortId { get; set; }
        public int PortAlternateId { get; set; }
        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Free { get; set; }
        public string TicketNo { get; set; }
        public string Remarks { get; set; }
        public string PostAt { get; set; }
        public string PostUser { get; set; }
        public string PutAt { get; set; }
        public string PutUser { get; set; }

    }

}
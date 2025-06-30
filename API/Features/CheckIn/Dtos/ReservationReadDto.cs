using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.CheckIn {

    public class ReservationReadDto {

        // PK
        public string ReservationId { get; set; }
        // Fields
        public string Date { get; set; }
        public string RefNo { get; set; }
        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Free { get; set; }
        public int TotalPax { get; set; }
        public string TicketNo { get; set; }
        public string Email { get; set; }
        public string Phones { get; set; }
        public string Remarks { get; set; }
        // Metadata
        public string PutAt { get; set; }
        //  Navigation
        public SimpleEntity Customer { get; set; }
        public SimpleEntity Destination { get; set; }
        public PickupPointReadDto PickupPoint { get; set; }
        public List<PassengerReadDto> Passengers { get; set; }

    }

}
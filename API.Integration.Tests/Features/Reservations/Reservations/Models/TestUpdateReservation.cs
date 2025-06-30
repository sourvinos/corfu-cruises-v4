using System;
using System.Collections.Generic;
namespace Reservations {

    public class TestUpdateReservation {

        public int StatusCode { get; set; }

        public Guid ReservationId { get; set; }
        public int CustomerId { get; set; }
        public int DestinationId { get; set; }
        public int PickupPointId { get; set; }
        public int PortId { get; set; }
        public int PortAlternateId { get; set; }
        public int? DriverId { get; set; }
        public int? ShipId { get; set; }
        public string Date { get; set; }
        public DateTime Now { get; set; }
        public string RefNo { get; set; }
        public string TicketNo { get; set; }
        public string Email { get; set; }
        public string Phones { get; set; }
        public int Adults { get; set; }
        public int Kids { get; set; }
        public int Free { get; set; }
        public string Remarks { get; set; }
        public string PutAt { get; set; }

        public List<TestPassenger> Passengers { get; set; }

    }

}
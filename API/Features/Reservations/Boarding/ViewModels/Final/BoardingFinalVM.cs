using System.Collections.Generic;
using API.Infrastructure.Classes;

namespace API.Features.Reservations.Boarding {

    public class BoardingFinalVM {

        public string RefNo { get; set; }
        public string TicketNo { get; set; }
        public string Remarks { get; set; }
        public SimpleEntity Customer { get; set; }
        public BoardingFinalDestinationListVM Destination { get; set; }
        public SimpleEntity Driver { get; set; }
        public SimpleEntity PickupPoint { get; set; }
        public SimpleEntity Port { get; set; }
        public BoardingFinalShipListVM Ship { get; set; }
        public int TotalPax { get; set; }
        public int EmbarkedPassengers { get; set; }
        public SimpleEntity BoardingStatus { get; set; }

        public int[] PassengerIds { get; set; }

        public List<BoardingFinalPassengerVM> Passengers { get; set; }

    }

}
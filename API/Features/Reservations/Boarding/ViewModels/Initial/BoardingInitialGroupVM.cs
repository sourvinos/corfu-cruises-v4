using System.Collections.Generic;
using API.Features.Reservations.Reservations;

namespace API.Features.Reservations.Boarding {

    public class BoardingInitialGroupVM {

        public int TotalPax { get; set; }
        public int EmbarkedPassengers { get; set; }
        public int PendingPax { get; set; }

        public List<Reservation> Reservations { get; set; }

    }

}
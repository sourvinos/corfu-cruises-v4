using System.Collections.Generic;

namespace API.Features.Reservations.Boarding {

    public class BoardingFinalGroupVM {

        public int TotalPax { get; set; }
        public int EmbarkedPassengers { get; set; }
        public int PendingPax { get; set; }

        public IEnumerable<BoardingFinalVM> Reservations { get; set; }

    }

}
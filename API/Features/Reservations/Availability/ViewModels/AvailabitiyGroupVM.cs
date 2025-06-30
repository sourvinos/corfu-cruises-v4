using System.Collections.Generic;

namespace API.Features.Reservations.Availability {

    public class AvailabilityGroupVM {

        public string Date { get; set; }
        public IEnumerable<DestinationCalendarVM> Destinations { get; set; }

    }

}
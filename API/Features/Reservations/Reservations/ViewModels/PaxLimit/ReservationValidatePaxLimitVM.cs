using API.Infrastructure.Classes;

namespace API.Features.Reservations.Reservations {

    public class ReservationValidatePaxLimitVM {

        public SimpleEntity Customer { get; set; }
        public int PaxLimit { get; set; }
        public int ExistingPax { get; set; }

    }

}
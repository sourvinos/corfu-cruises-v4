using API.Features.Reservations.Reservations;

namespace API.Features.CheckIn {

    public interface ICheckInValidation {

        int IsValidOnRead(Reservation x);
        int IsValidOnUpdate(Reservation x, ReservationWriteDto reservation);

    }

}
using System.Threading.Tasks;
using API.Features.Reservations.Schedules;

namespace API.Features.Reservations.Reservations {

    public interface IReservationValidation {

        bool IsUserOwner(int customerId);
        Task<bool> IsKeyUnique(ReservationWriteDto reservation);
        int GetPortIdFromPickupPointId(int pickupPointId);
        int OverbookedPax(string date, int destinationId);
        Task<int> IsValidAsync(Reservation x, ReservationWriteDto reservation, IScheduleRepository scheduleRepo);

    }

}
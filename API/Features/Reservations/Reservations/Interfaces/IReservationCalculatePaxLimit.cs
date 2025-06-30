using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Reservations {

    public interface IReservationCalculatePaxLimit : IRepository<Reservation> {

        int CalculateExistingPax(int customerId, string date);

    }

}
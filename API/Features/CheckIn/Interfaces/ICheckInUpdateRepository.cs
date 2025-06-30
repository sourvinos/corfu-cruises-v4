using System;
using API.Features.Reservations.Reservations;

namespace API.Features.CheckIn {

    public interface ICheckInUpdateRepository {

        Reservation Update(Guid id, Reservation reservation);
        void UpdateEmail(Reservation reservation, string email);

    }

}
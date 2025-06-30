using System;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Reservations {

    public interface IReservationUpdateRepository : IRepository<Reservation> {

        Reservation Update(Guid id, Reservation reservation);
        void AssignToDriver(int driverId, string[] reservationIds);
        void AssignToPort(int portId, string[] reservationIds);
        void AssignToShip(int shipId, string[] reservationIds);
        string AssignRefNoToNewDto(ReservationWriteDto reservation);
        void DeleteRange(string[] ids);

    }

}
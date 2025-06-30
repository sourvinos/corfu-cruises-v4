using System.Collections.Generic;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Reservations {

    public interface IReservationCloneRepository : IRepository<Reservation> {

        int Clone(IEnumerable<CloneReservationVM> reservations);

    }

}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Infrastructure.Interfaces;

namespace API.Features.Reservations.Reservations {

    public interface IReservationReadRepository : IRepository<Reservation> {

        IQueryable<ReservationListVM> GetByDateAsync(string date);
        IQueryable<ReservationListVM> GetByRefNoAsync(string refNo);
        Task<ReservationDriverGroupVM> GetByDateAndDriverAsync(string date, int driverId);
        Task<Reservation> GetByIdAsync(string reservationId, bool includeTables);
        Task<Reservation> GetByIdForPatchEmailSent(string reservationId);

    }

}